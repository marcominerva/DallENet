using DallENet.Exceptions;
using DallENet.Models;
using DallENet.ServiceConfigurations;

namespace DallENet;

/// <summary>
/// Builder class to define settings for configuring DALL·E.
/// </summary>
public class DallEOptionsBuilder
{
    /// <summary>
    /// Gets or sets the configuration settings for accessing the service.
    /// </summary>
    /// <seealso cref="DallEServiceConfiguration"/>
    /// <seealso cref="AzureDallEServiceConfiguration"/>
    internal DallEServiceConfiguration ServiceConfiguration { get; set; } = default!;

    /// <summary>
    /// Gets or sets the default resolution for image generation (default: 1024x1024).
    /// </summary>
    public string DefaultResolution { get; set; } = DallEImageResolutions.Large;

    /// <summary>
    /// Gets or sets the default number of images to generate for a request. Must be between 1 and 5 (default: 1).
    /// </summary>
    public int DefaultImageCount { get; set; } = 1;

    /// <summary>
    /// Gets or sets a value that determines whether to throw a <see cref="DallEException"/> when an error occurred (default: <see langword="true"/>). If this property is set to <see langword="false"></see>, API errors are returned in the <see cref="DallEImageGenerationResponse"/> object.
    /// </summary>
    /// <seealso cref="DallEException"/>
    /// <seealso cref="DallEImageGenerationResponse"/>
    public bool ThrowExceptionOnError { get; set; } = true;

    internal DallEOptions Build()
        => new()
        {
            DefaultResolution = DefaultResolution,
            DefaultImageCount = DefaultImageCount,
            ThrowExceptionOnError = ThrowExceptionOnError,
            ServiceConfiguration = ServiceConfiguration
        };
}
