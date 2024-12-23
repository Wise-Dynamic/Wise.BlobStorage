using BlobStorage.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Wise.BlobStorage.Domain.Models;
using Wise.BlobStorage.Infrastructure.Context;

namespace BlobStorage.Providers
{
    public class BlobProviderFactory : IBlobProviderFactory
    {
        private readonly BlobStorageOption _options;
        private readonly IServiceProvider _serviceProvider;

        public BlobProviderFactory(IOptions<BlobStorageOption> options, IServiceProvider serviceProvider)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        public IBlobProvider Create()
        {

            var providerName = _options.Provider;

            return _options.Provider switch
            {
                "FileSystem" => CreateFileSystemProvider(),
                "Azure" => CreateAzureProvider(),
                "Database" => CreateDatabaseProvider(),    
            };
        }

        public IBlobProvider CreateFileSystemProvider()
        {
            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            var basePath = configuration.GetSection("BlobStorage:FileSystem:BasePath").Value;
            basePath = Path.Combine(Directory.GetCurrentDirectory(), basePath);
            return new FileSystemBlobProvider(basePath);
        }
       
        private IBlobProvider CreateDatabaseProvider()
        {
            var dbContext = _serviceProvider.GetRequiredService<WiseDbContext>();
            return new DatabaseBlobProvider(dbContext);
        }

        private IBlobProvider CreateAzureProvider()
        {
            //var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            //var connectionString = configuration.GetSection("BlobStorage:Azure:ConnectionString").Value;
            return new AzureBlobProvider();
        }

    }
}
