using System.Text.Json.Serialization;
using DallENet.Models.Converters;

namespace DallENet.Models;

/// <summary>
/// Represents an image generation result.
/// </summary>
public class DallEImageGenerationResult
{
    /// <summary>
    /// Gets or sets the UTC date and time at which the result has been generated.
    /// </summary>
    [JsonPropertyName("created")]
    [JsonConverter(typeof(UnixToDateTimeConverter))]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the images associated with the current result.
    /// </summary>
    [JsonPropertyName("data")]
    public IEnumerable<DallEImage> Images { get; set; } = Enumerable.Empty<DallEImage>();
}
