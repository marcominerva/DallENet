using Microsoft.Extensions.DependencyInjection;

namespace DallENet;

/// <summary>
/// Represents a builder for configuring DALLE·E.
/// </summary>
public interface IDallEBuilder
{
    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> where DALLE·E services are registered.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Gets the <see cref="IHttpClientBuilder"/> used to configure the <see cref="HttpClient"/> used by DALLE·E.
    /// </summary>
    IHttpClientBuilder HttpClientBuilder { get; }
}
