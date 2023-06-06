using System.Net.Http.Json;
using System.Text.Json;
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

    public async Task<DallEImageGenerationResponse> GenerateImageAsync(string prompt, int? imageCount = null, string? resolution = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(prompt);

        var request = CreateRequest(prompt, imageCount, resolution);

        var requestUri = options.ServiceConfiguration.GetImageGenerationEndpoint();
        using var httpResponse = await httpClient.PostAsJsonAsync<object>(requestUri, request, cancellationToken);

        DallEImageGenerationResponse? response = null;
        if (httpResponse.IsSuccessStatusCode)
        {
            var operationLocation = httpResponse.Headers.GetValues("Operation-Location").FirstOrDefault();
            var retryAfter = httpResponse.Headers.RetryAfter?.Delta ?? TimeSpan.FromSeconds(10);

            using var responseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(responseStream, cancellationToken: cancellationToken);

            var operationId = document.RootElement.GetProperty("id").GetString();

            // Waits until the actual response (with images URL) is available.
            var isWorking = true;
            while (isWorking)
            {
                await Task.Delay(retryAfter, cancellationToken);
                response = await httpClient.GetFromJsonAsync<DallEImageGenerationResponse>(operationLocation, cancellationToken);

                isWorking = response!.Status is "notRunning" or "running";
            }

            response!.OperationId = operationId ?? string.Empty;
            EnsureErrorIsSet(response, httpResponse);
        }
        else
        {
            response = await httpResponse.Content.ReadFromJsonAsync<DallEImageGenerationResponse>(cancellationToken: cancellationToken);
            EnsureErrorIsSet(response!, httpResponse);

            if (options.ThrowExceptionOnError)
            {
                throw new DallEException(response!.Error, httpResponse.StatusCode);
            }
        }

        return response!;
    }

    private DallEImageGenerationRequest CreateRequest(string prompt, int? imageCount = null, string? resolution = null)
        => new()
        {
            Prompt = prompt,
            Resolution = resolution ?? options.DefaultResolution,
            ImageCount = imageCount ?? options.DefaultImageCount
        };

    private static void EnsureErrorIsSet(DallEImageGenerationResponse response, HttpResponseMessage httpResponse)
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
