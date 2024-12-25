using BlobStorage.Interfaces;
using BlobStorage.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wise.BlobStorage.Infrastructure.Context;
using Wise.BlobStorage.Infrastructure.Providers;

namespace Wise.BlobStorage.Infrastructure
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
        {
            
            //services.AddDbContext<WiseDbContext>(options =>
            //{
            //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            //});

            services.AddScoped<IBlobProviderFactoryService, BlobProviderFactoryService>();
            services.AddScoped<IBlobService, BlobService>();

            return services;
        }
    }
}
