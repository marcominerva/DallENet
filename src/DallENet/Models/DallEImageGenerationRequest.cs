using System.Text.Json.Serialization;

namespace DallENet.Models;

internal class DallEImageGenerationRequest
{
    public string? Prompt { get; set; }

    [JsonPropertyName("size")]
    public string Resolution { get; set; } = DallEImageResolutions.Large;

    [JsonPropertyName("n")]
    public int ImageCount { get; set; } = 1;
}
