using System.Net;
using System.Net.Http.Json;
using DallENet.Exceptions;
using DallENet.Extensions;
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

    public async Task<Stream> GetImageStreamAsync(string prompt, string? size = null, string? quality = null, string? style = null, string? model = null, CancellationToken cancellationToken = default)
    {
        var response = await GenerateImagesAsync(prompt, size, quality, style, DallEImageResponseFormats.Url, model, cancellationToken);

        var stream = await GetImageStreamAsync(response, cancellationToken);
        return stream;
    }

    public async Task<Stream> GetImageStreamAsync(DallEImageGenerationResponse response, CancellationToken cancellationToken = default)
    {
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

    public async Task<DallEImageGenerationResponse> GenerateImagesAsync(string prompt, string? size = null, string? quality = null, string? style = null, string? imageResponseFormat = null, string? model = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(prompt);

        var request = CreateRequest(prompt, size, quality, imageResponseFormat, style);

        var requestUri = options.ServiceConfiguration.GetImageGenerationEndpoint(model ?? options.DefaultModel);
        using var httpResponse = await httpClient.PostAsJsonAsync(requestUri, request, cancellationToken);

        var response = await httpResponse.Content.ReadFromJsonAsync<DallEImageGenerationResponse>(cancellationToken: cancellationToken);
        NormalizeRenspose(response!, httpResponse);

        if (!response!.IsSuccessful && options.ThrowExceptionOnError)
        {
            throw new DallEException(response!.Error, httpResponse.StatusCode);
        }

        return response;
    }

    private DallEImageGenerationRequest CreateRequest(string prompt, string? size, string? quality, string? imageResponseFormat, string? style)
        => new()
        {
            Prompt = prompt,
            Size = size ?? options.DefaultSize,
            Quality = quality ?? options.DefaultQuality,
            ResponseFormat = imageResponseFormat ?? options.DefaultResponseFormat,
            Style = style ?? options.DefaultStyle
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
