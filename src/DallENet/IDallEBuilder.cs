using Microsoft.Extensions.DependencyInjection;

namespace DallENet;

/// <summary>
/// Represents a builder for configuring DALL·E.
/// </summary>
public interface IDallEBuilder
{
    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> where DALL·E services are registered.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Gets the <see cref="IHttpClientBuilder"/> used to configure the <see cref="HttpClient"/> used by DALL·E.
    /// </summary>
    IHttpClientBuilder HttpClientBuilder { get; }
}
