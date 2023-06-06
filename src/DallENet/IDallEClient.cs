using DallENet.Exceptions;
using DallENet.Models;

namespace DallENet;

/// <summary>
/// Provides methods to interact with DALLE·E.
/// </summary>
public interface IDallEClient
{
    /// <summary>
    /// Requests a new image generation.
    /// </summary>
    /// <param name="prompt">A text description of the desired image(s). The maximum length is 1000 characters.</param>
    /// <param name="imageCount">The number of images to generate. Must be between 1 and 5. If <see langword="null"/>, the number set in the <see cref="DallEOptions.DefaultImageCount"/> will be used (default: 1).</param>
    /// <param name="resolution">The size of the generated images. If <see langword="null"/>, the resolution set in the <see cref="DallEOptions.DefaultResolution"/> will be used (default: 1024x1024).</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>The image generation response.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="prompt"/> is <see langword="null"/>.</exception>
    /// <exception cref="DallEException">An error occurred while calling the API and the <see cref="DallEOptions.ThrowExceptionOnError"/> is <see langword="true"/>.</exception>
    /// <seealso cref="DallEImageGenerationResponse"/>
    /// <seealso cref="DallEOptions"/>
    /// <seealso cref="DallEException"/>
    Task<DallEImageGenerationResponse> GenerateImageAsync(string prompt, int? imageCount = null, string? resolution = null, CancellationToken cancellationToken = default);
}
