using AviasalesApi.Services.AirlineAdapters;

namespace AviasalesApi.Extensions
{
    public static class AdapterInjectExtensions
    {
        public static void AddAdapters(this IServiceCollection services)
        {
            var adapters = typeof(Program).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(IAirlineAdapter)) == typeof(IAirlineAdapter));

            foreach (var adapter in adapters)
            {
                services.Add(new ServiceDescriptor(typeof(IAirlineAdapter), adapter, ServiceLifetime.Singleton));
            }
        }
    }
}
