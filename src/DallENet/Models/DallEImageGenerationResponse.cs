using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using DallENet.Models.Converters;

namespace DallENet.Models;

/// <summary>
/// Represents an image generation respose.
/// </summary>
/// <remarks>
/// Generated images are automatically deleted after 24 hours.
/// </remarks>
public class DallEImageGenerationResponse
{
    /// <summary>
    /// Gets or sets the UTC date and time at which the response has been generated.
    /// </summary>
    [JsonPropertyName("created")]
    [JsonConverter(typeof(UnixToDateTimeConverter))]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the error occurred during the image generation execution, if any.
    /// </summary>
    public DallEError? Error { get; set; }

    /// <summary>
    /// Gets a value that determines if the response was successful.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccessful => Error is null;

    /// <summary>
    /// Gets or sets the image that has been generated.
    /// </summary>
    [JsonPropertyName("data")]
    public IEnumerable<DallEImage> Images { get; set; } = Enumerable.Empty<DallEImage>();
}