using System.Text.Json.Serialization;

namespace DallENet.Models;

internal class DallEImageGenerationRequest
{
    public string Prompt { get; set; } = null!;

    [JsonPropertyName("size")]
    public string Size { get; set; } = DallEImageSizes._1024x1024;

    public string Quality { get; set; } = DallEImageQualities.Standard;

    [JsonPropertyName("response_format")]
    public string ResponseFormat { get; set; } = DallEImageResponseFormats.Url;

    public string Style { get; set; } = DallEImageStyles.Vivid;
}
