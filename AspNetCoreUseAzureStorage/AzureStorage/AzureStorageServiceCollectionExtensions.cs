using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetCoreUseAzureStorage.AzureStorage
{
    public static class AzureStorageServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureStorage(this IServiceCollection services, Action<AzureStorageOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddOptions();
            services.Configure(setupAction);
            services.AddScoped(typeof(AzureStorageRepository));

            return services;
        }
    }
}
