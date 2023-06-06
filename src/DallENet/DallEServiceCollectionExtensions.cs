using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DallENet;

/// <summary>
/// Provides extension methods for adding DALLE·E support in .NET applications.
/// </summary>
public static class DallEServiceCollectionExtensions
{
    /// <summary>
    /// Registers a <see cref="DallEClient"/> instance with the specified options.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="builder">The <see cref="DallEOptionsBuilder"/> to configure options.</param>
    /// <returns>A <see cref="IDallEBuilder"/> that can be used to further customize DALLE·E.</returns>
    /// <seealso cref="DallEOptionsBuilder"/>
    /// <seealso cref="IDallEBuilder"/>
    public static IDallEBuilder AddDallE(this IServiceCollection services, Action<DallEOptionsBuilder> builder)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(builder);

        var options = new DallEOptionsBuilder();
        builder.Invoke(options);

        ArgumentNullException.ThrowIfNull(options.ServiceConfiguration);

        services.AddSingleton(options.Build());

        return AddDallECore(services);
    }

    /// <summary>
    /// Registers a <see cref="DallEClient"/> instance reading configuration from the specified <see cref="IConfiguration"/> source.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> being bound.</param>
    /// <param name="sectionName">The name of the configuration section that holds DALLE·E settings (default: DallE).</param>
    /// <returns>A <see cref="IDallEBuilder"/> that can be used to further customize DALLE·E.</returns>
    /// <seealso cref="DallEOptions"/>
    /// <seealso cref="IConfiguration"/>
    /// <seealso cref="IDallEBuilder"/>
    public static IDallEBuilder AddDallE(this IServiceCollection services, IConfiguration configuration, string sectionName = "DallE")
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var options = new DallEOptionsBuilder();
        options.UseConfiguration(configuration, sectionName);

        services.AddSingleton(options.Build());

        return AddDallECore(services);
    }

    /// <summary>
    /// Registers a <see cref="DallEClient"/> instance using dynamic options.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="builder">The <see cref="DallEOptionsBuilder"/> to configure options.</param>
    /// <returns>A <see cref="IDallEBuilder"/> that can be used to further customize DALLE·E.</returns>
    /// <remarks>Use this this method if it is necessary to dynamically set options (for example, using other services via dependency injection).
    /// </remarks>
    /// <seealso cref="DallEOptions"/>
    /// <seealso cref="IServiceProvider"/>
    /// <seealso cref="IDallEBuilder"/>
    public static IDallEBuilder AddDallE(this IServiceCollection services, Action<IServiceProvider, DallEOptionsBuilder> builder)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(builder);

        services.AddScoped(provider =>
        {
            var options = new DallEOptionsBuilder();
            builder.Invoke(provider, options);

            ArgumentNullException.ThrowIfNull(options.ServiceConfiguration);

            return options.Build();
        });

        return AddDallECore(services);
    }

    private static IDallEBuilder AddDallECore(IServiceCollection services)
    {
        var httpClientBuilder = services.AddHttpClient<IDallEClient, DallEClient>();
        return new DallEBuilder(services, httpClientBuilder);
    }
}
