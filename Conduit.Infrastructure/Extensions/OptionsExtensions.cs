using Conduit.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Infrastructure.Extensions;

internal static class OptionsExtensions
{
    internal static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DownstreamOptions>(configuration.GetSection("DownstreamOptions"));
    }
}