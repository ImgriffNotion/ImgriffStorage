using ImgriffStorage.Services.AzureBlobStorageServices;
using ImgriffStorage.Services.HashServices;

namespace ImgriffStorage.Services
{
    public static class ImgriffStorageServiceExtentions
    {
        public static IServiceCollection RegistrateServices(this IServiceCollection services)
        {
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<IBlobService, BlobService>();

            return services;
        }
    }
}
