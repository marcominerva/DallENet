using System.Text.Json.Serialization;

namespace DallENet.Models;

/// <summary>
/// Represents an image.
/// </summary>
/// <remarks>
/// Generated images are automatically deleted after 24 hours.
/// </remarks>
public class DallEImage
{
    /// <summary>
    /// Gets or sets the url of the image.
    /// </summary>
    /// <remarks>
    /// Generated images are automatically deleted after 24 hours.
    /// </remarks>
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the base64 byte code of the image in JSON format.
    /// </summary>
    [JsonPropertyName("b64_json")]
    public string? Base64Json { get; set; }

    /// <summary>
    /// Gets or sets the actual prompt that has been used to generate the image.
    /// </summary>
    [JsonPropertyName("revised_prompt")]
    public string RevisedPrompt { get; set; } = string.Empty;
}