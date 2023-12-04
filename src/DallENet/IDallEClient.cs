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
    /// <param name="prompt">A text description of the desired image. The maximum length is 4000 characters.</param>
    /// <param name="size">The size of the generated image. If <see langword="null"/>, the size set in the <see cref="DallEOptions.DefaultSize"/> property will be used (default: <see cref="DallEImageSizes._1024x1024"/>).</param>
    /// <param name="quality">The quality of the generated image. If <see langword="null"/>, the quality set in the <see cref="DallEOptions.DefaultQuality"/> property will be used (default: <see cref="DallEImageQualities.Standard"/>).</param>
    /// <param name="style">The style of the generated image. If <see langword="null"/>, the style set in the <see cref="DallEOptions.DefaultStyle"/> property will be used (default: <see cref="DallEImageStyles.Vivid"/>).</param>
    /// <param name="imageResponseFormat">The format in which the generated images are returned. If <see langword="null"/>, the format set in the <see cref="DallEOptions.DefaultResponseFormat"/> property will be used (default: <see cref="DallEImageResponseFormats.Url"/>).</param>
    /// <param name="model">The image generation model to use. If <see langword="null"/>, the model specified in the <see cref="DallEOptions.DefaultModel"/> property will be used.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The image generation response.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="prompt"/> is <see langword="null"/>.</exception>
    /// <exception cref="DallEException">An error occurred while calling the API and the <see cref="DallEOptions.ThrowExceptionOnError"/> is <see langword="true"/>.</exception>
    /// <seealso cref="DallEImageGenerationResponse"/>
    /// <seealso cref="DallEOptions"/>    
    /// <seealso cref="DallEException"/>
    Task<DallEImageGenerationResponse> GenerateImagesAsync(string prompt, string? size = null, string? quality = null, string? style = null, string? imageResponseFormat = null, string? model = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Requests a new image generation and directly returns the <see cref="Stream"/> containing the image.
    /// </summary>
    /// <param name="prompt">A text description of the desired image(s). The maximum length is 4000 characters.</param>
    /// <param name="size">The size of the generated image. If <see langword="null"/>, the size set in the <see cref="DallEOptions.DefaultSize"/> property will be used (default: <see cref="DallEImageSizes._1024x1024"/>).</param>
    /// <param name="quality">The quality of the generated image. If <see langword="null"/>, the quality set in the <see cref="DallEOptions.DefaultQuality"/> property will be used (default: <see cref="DallEImageQualities.Standard"/>).</param>
    /// <param name="style">The style of the generated image. If <see langword="null"/>, the style set in the <see cref="DallEOptions.DefaultStyle"/> property will be used (default: <see cref="DallEImageStyles.Vivid"/>).</param>
    /// <param name="model">The image generation model to use. If <see langword="null"/>, the model specified in the <see cref="DallEOptions.DefaultModel"/> property will be used.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The <see cref="Stream"/> containing the image</returns>
    /// <exception cref="ArgumentNullException"><paramref name="prompt"/> is <see langword="null"/>.</exception>
    /// <exception cref="DallEException">An error occurred while calling the API.</exception>
    /// <remarks>If an error occurred, this method throws a <see cref="DallEException"/> no matter the value of the <see cref="DallEOptions.ThrowExceptionOnError"/> property.</remarks>
    /// <seealso cref="Stream"/>
    /// <seealso cref="DallEOptions"/>
    /// <seealso cref="DallEException"/>
    Task<Stream> GetImageStreamAsync(string prompt, string? size = null, string? quality = null, string? style = null, string? model = null, CancellationToken cancellationToken = default);

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
}
