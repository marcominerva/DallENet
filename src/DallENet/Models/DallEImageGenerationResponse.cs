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
    /// Gets or sets the UTC date and time at which the response will expire.
    /// </summary>
    [JsonPropertyName("expires")]
    [JsonConverter(typeof(UnixToDateTimeConverter))]
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the Id of the response.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status of the response.
    /// </summary>
    /// <remarks>
    ///  Possible values are: <em>notRunning</em> (task is queued but hasn't started yet), <em>running</em>, <em>succeeded</em>, <em>canceled</em> (task has timed out), <em>failed</em>, or <em>deleted</em>.
    /// </remarks>
    public string Status { get; set; } = string.Empty;

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
    /// Gets or sets the result that has been generated.
    /// </summary>
    public DallEImageGenerationResult Result { get; set; } = new();

    /// <summary>
    /// Gets the URL of the specified image, if available.
    /// </summary>
    /// <param name="index">The index of the image to get the URL for (default: 0).</param>
    /// <returns>The URL of the first image, if available.</returns>
    public string? GetImageUrl(int index = 0) => Result.Images.ElementAtOrDefault(index)?.Url;
}