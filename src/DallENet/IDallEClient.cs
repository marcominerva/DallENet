using DallENet.Exceptions;
using DallENet.Models;

namespace DallENet;

/// <summary>
/// Provides methods to interact with DALL·E.
/// </summary>
public interface IDallEClient
{
    /// <summary>
    /// Requests a new images generation.
    /// </summary>
    /// <param name="prompt">A text description of the desired image(s). The maximum length is 1000 characters.</param>
    /// <param name="imageCount">The number of images to generate. Must be between 1 and 5. If <see langword="null"/>, the number set in the <see cref="DallEOptions.DefaultImageCount"/> will be used (default: 1).</param>
    /// <param name="resolution">The size of the generated images. If <see langword="null"/>, the resolution set in the <see cref="DallEOptions.DefaultResolution"/> will be used (default: 1024x1024).</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The image generation response.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="prompt"/> is <see langword="null"/>.</exception>
    /// <exception cref="DallEException">An error occurred while calling the API and the <see cref="DallEOptions.ThrowExceptionOnError"/> is <see langword="true"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="imageCount"/> is not between 1 and 5.</exception>
    /// <seealso cref="DallEImageGenerationResponse"/>
    /// <seealso cref="DallEOptions"/>
    /// <seealso cref="DallEException"/>
    Task<DallEImageGenerationResponse> GenerateImagesAsync(string prompt, int? imageCount = null, string? resolution = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Requests a new image generation and directly returns the <see cref="Stream"/> containing the image.
    /// </summary>
    /// <param name="prompt">A text description of the desired image(s). The maximum length is 1000 characters.</param>
    /// <param name="resolution">The size of the generated images. If <see langword="null"/>, the resolution set in the <see cref="DallEOptions.DefaultResolution"/> will be used (default: 1024x1024).</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The <see cref="Stream"/> containing the image</returns>
    /// <exception cref="ArgumentNullException"><paramref name="prompt"/> is <see langword="null"/>.</exception>
    /// <exception cref="DallEException">An error occurred while calling the API.</exception>
    /// <remarks>If an error occurred, this method throws a <see cref="DallEException"/> no matter the value of the <see cref="DallEOptions.ThrowExceptionOnError"/> property.</remarks>
    /// <seealso cref="Stream"/>
    /// <seealso cref="DallEOptions"/>
    /// <seealso cref="DallEException"/>
    Task<Stream> GetImageStreamAsync(string prompt, string? resolution = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the <see cref="Stream"/> containing the image at the specified <paramref name="index"/> in the response.
    /// </summary>
    /// <param name="response">The response of a previous image generation request</param>
    /// <param name="index">The index of the image to get the <see cref="Stream"/> for (default: 0)..</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The <see cref="Stream"/> containing the image</returns>
    /// <remarks>If an error occurred, this method throws a <see cref="DallEException"/> no matter the value of the <see cref="DallEOptions.ThrowExceptionOnError"/> property.</remarks>
    /// <seealso cref="GenerateImagesAsync(string, int?, string?, CancellationToken)"/>
    /// <seealso cref="GetImageStreamAsync(string, string?, CancellationToken)"/>
    /// <seealso cref="Stream"/>
    /// <seealso cref="DallEException"/>
    Task<Stream> GetImageStreamAsync(DallEImageGenerationResponse response, int index = 0, CancellationToken cancellationToken = default);

    /// <summary>
    /// Triggers the deletion of generated images.
    /// </summary>
    /// <param name="operationId">The GUID that identifies the original image generation request.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The <see cref="Task"/> corresponding to the asynchronous operation.</returns>
    /// <exception cref="DallEException">An error occurred while calling the API.</exception>
    /// <remarks>
    /// Generated images are automatically deleted after 24 hours by default, but this method can be used to explicitly trigger the deletion earlier.
    /// This method deletes all the images that have been generated in the request associated with the given <paramref name="operationId"/>.
    /// </remarks>
    Task DeleteImagesAsync(string operationId, CancellationToken cancellationToken = default);
}
