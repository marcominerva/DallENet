using System.Net;
using System.Net.Http.Json;
using DallENet.Exceptions;
using DallENet.Models;

namespace DallENet;

internal class DallEClient : IDallEClient
{
    private readonly HttpClient httpClient;
    private readonly DallEOptions options;

    public DallEClient(HttpClient httpClient, DallEOptions options)
    {
        this.httpClient = httpClient;

        foreach (var header in options.ServiceConfiguration.GetRequestHeaders())
        {
            this.httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
        }

        this.options = options;
    }

    public async Task<Stream> GetImageStreamAsync(string prompt, string? resolution = null, CancellationToken cancellationToken = default)
    {
        // When requesting an image stream, always generate a single image.
        var response = await GenerateImagesAsync(prompt, 1, resolution, cancellationToken);

        if (response.IsSuccessful)
        {
            var imageUrl = response.GetImageUrl();
            var imageStream = await httpClient.GetStreamAsync(imageUrl, cancellationToken);

            return imageStream;
        }

        // If there was an error, always throws an exception, even if the "DallEOptions.ThrowExceptionOnError" property is false.
        _ = int.TryParse(response.Error!.Code, out var code);
        throw new DallEException(response!.Error, (HttpStatusCode)code);
    }

    public async Task<DallEImageGenerationResponse> GenerateImagesAsync(string prompt, int? imageCount = null, string? resolution = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(prompt);

        var request = CreateRequest(prompt, imageCount, resolution);

        if (request.ImageCount is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(imageCount), "The number of images to generate must be between 1 and 5.");
        }

        var requestUri = options.ServiceConfiguration.GetImageGenerationEndpoint();
        using var httpResponse = await httpClient.PostAsJsonAsync(requestUri, request, cancellationToken);

        DallEImageGenerationResponse? response = null;
        if (httpResponse.IsSuccessStatusCode)
        {
            var operationLocation = httpResponse.Headers.GetValues("Operation-Location").FirstOrDefault();
            var retryAfter = httpResponse.Headers.RetryAfter?.Delta ?? TimeSpan.FromSeconds(10);

            // Waits until the actual response (with images URL) is available.
            var isRunning = true;
            while (isRunning)
            {
                await Task.Delay(retryAfter, cancellationToken);

                response = await httpClient.GetFromJsonAsync<DallEImageGenerationResponse>(operationLocation, cancellationToken);
                NormalizeRenspose(response!, httpResponse);

                isRunning = response!.Status is "notRunning" or "running";
            }
        }
        else
        {
            response = await httpResponse.Content.ReadFromJsonAsync<DallEImageGenerationResponse>(cancellationToken: cancellationToken);
            NormalizeRenspose(response!, httpResponse);
        }

        if (!response!.IsSuccessful && options.ThrowExceptionOnError)
        {
            throw new DallEException(response!.Error, httpResponse.StatusCode);
        }

        return response;
    }

    public async Task DeleteImagesAsync(string operationId, CancellationToken cancellationToken = default)
    {
        var requestUri = options.ServiceConfiguration.GetDeleteImageEndpoint(operationId);
        using var httpResponse = await httpClient.DeleteAsync(requestUri, cancellationToken);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var response = await httpResponse.Content.ReadFromJsonAsync<DallEImageGenerationResponse>(cancellationToken: cancellationToken);
            NormalizeRenspose(response!, httpResponse);

            throw new DallEException(response!.Error, httpResponse.StatusCode);
        }
    }

    private DallEImageGenerationRequest CreateRequest(string prompt, int? imageCount = null, string? resolution = null)
        => new()
        {
            Prompt = prompt,
            Resolution = resolution ?? options.DefaultResolution,
            ImageCount = imageCount ?? options.DefaultImageCount
        };

    private static void NormalizeRenspose(DallEImageGenerationResponse response, HttpResponseMessage httpResponse)
    {
        if (!httpResponse.IsSuccessStatusCode && response.Error is null)
        {
            response.Error = new DallEError
            {
                Message = httpResponse.ReasonPhrase ?? httpResponse.StatusCode.ToString(),
                Code = ((int)httpResponse.StatusCode).ToString()
            };
        }
    }
}
