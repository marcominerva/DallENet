using Microsoft.Extensions.DependencyInjection;

namespace DallENet;

/// <inheritdoc/>
public class DallEBuilder : IDallEBuilder
{
    /// <inheritdoc/>
    public IServiceCollection Services { get; }

    /// <inheritdoc/>
    public IHttpClientBuilder HttpClientBuilder { get; }

    internal DallEBuilder(IServiceCollection services, IHttpClientBuilder httpClientBuilder)
    {
        Services = services;
        HttpClientBuilder = httpClientBuilder;
    }
}
