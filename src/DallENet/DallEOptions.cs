using DallENet.Exceptions;
using DallENet.Models;
using DallENet.ServiceConfigurations;

namespace DallENet;

/// <summary>
/// Options class that provides settings for configuring DALL·E.
/// </summary>
public class DallEOptions
{
    /// <summary>
    /// Gets or sets the configuration settings for accessing the service.
    /// </summary>
    /// <seealso cref="DallEServiceConfiguration"/>
    /// <seealso cref="AzureDallEServiceConfiguration"/>
    internal DallEServiceConfiguration ServiceConfiguration { get; set; } = default!;

    /// <summary>
    /// Gets or sets the default model for image generation.
    /// </summary>
    public string? DefaultModel { get; set; }

    /// <summary>
    /// Gets or sets the default resolution for image generation (default: <see cref="DallEImageSizes._1024x1024"/>).
    /// </summary>
    /// <seealso cref="DallEImageSizes"/>
    public string DefaultSize { get; set; } = DallEImageSizes._1024x1024;

    /// <summary>
    /// Gets or sets the default quality of generated images (default: <see cref="DallEImageQualities.Standard"/>.
    /// </summary>
    /// <seealso cref="DallEImageQualities"/>
    public string DefaultQuality { get; set; } = DallEImageQualities.Standard;

    /// <summary>
    /// Gets or sets the default format of generated images (default: <see cref="DallEImageResponseFormats.Url"/>.
    /// </summary>
    /// <seealso cref="DallEImageResponseFormats"/>
    public string DefaultResponseFormat { get; set; } = DallEImageResponseFormats.Url;

    /// <summary>
    /// Gets or sets the default style of generated images (default: <see cref="DallEImageStyles.Vivid"/>.
    /// </summary>
    /// <seealso cref="DallEImageStyles"/>
    public string DefaultStyle { get; set; } = DallEImageStyles.Vivid;

    /// <summary>
    /// Gets or sets the user identification for chat completion, which can help OpenAI to monitor and detect abuse.
    /// </summary>
    /// <remarks>
    /// See <see href="https://platform.openai.com/docs/guides/safety-best-practices/end-user-ids">Safety best practices</see> for more information.
    /// </remarks>
    public string? User { get; set; }

    /// <summary>
    /// Gets or sets a value that determines whether to throw a <see cref="DallEException"/> when an error occurred (default: <see langword="true"/>). If this property is set to <see langword="false"></see>, API errors are returned in the <see cref="DallEImageGenerationResponse"/> object.
    /// </summary>
    /// <seealso cref="DallEException"/>
    /// <seealso cref="DallEImageGenerationResponse"/>
    public bool ThrowExceptionOnError { get; set; } = true;
}
