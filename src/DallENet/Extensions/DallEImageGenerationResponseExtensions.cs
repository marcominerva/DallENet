using DallENet.Models;

namespace DallENet.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="DallEImageGenerationResponse"/> class.
/// </summary>
/// <seealso cref="DallEImageGenerationResponse"/>
public static class DallEImageGenerationResponseExtensions
{
    /// <summary>
    /// Gets the image that has been generated.
    /// </summary>
    /// <returns>The generated image, if available.</returns>
    public static DallEImage? GetImage(this DallEImageGenerationResponse response)
        => response.Images.FirstOrDefault();

    /// <summary>
    /// Gets the URL of the generated image.
    /// </summary>
    /// <returns>The URL of the generated image, if available.</returns>
    public static string? GetImageUrl(this DallEImageGenerationResponse response)
        => response.GetImage()?.Url;

    /// <summary>
    /// Gets the Base64 encoding of the generated image.
    /// </summary>
    /// <returns>The Base64 encocind of the generated image, if available.</returns>
    public static string? GetImageBase64(this DallEImageGenerationResponse response)
        => response.GetImage()?.Base64;
}
