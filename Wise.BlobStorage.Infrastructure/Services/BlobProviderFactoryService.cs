using BlobStorage.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Wise.BlobStorage.Domain.Entities;
using Wise.BlobStorage.Domain.Models;
using Wise.BlobStorage.Infrastructure.Context;

namespace BlobStorage.Providers
{
    public class BlobProviderFactoryService : IBlobProviderFactoryService
    {
        private readonly BlobStorageOption _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly WiseDbContext _context;

        public BlobProviderFactoryService(IOptions<BlobStorageOption> options, IServiceProvider serviceProvider, WiseDbContext context)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _context = context;
        }

        public async Task<(IBlobProviderService, Blob)> GetProvider(long blobId)
        {
            var blob = await _context.Blobs.FirstOrDefaultAsync(x => x.Id == blobId && !x.IsDeleted);
            if (blob == null)
            {
                throw new Exception("Blob not found");
            }

            return blob.BlobType switch
            {
                BlobType.FileSystem => (CreateFileSystemProvider(), blob),
                BlobType.Azure => (CreateAzureProvider(), blob),
                BlobType.Database => (CreateDatabaseProvider(), blob),
                _ => throw new Exception("Unsupported BlobType")
            };
        }

        public IBlobProviderService Create()
        {
            var providerName = _options.Provider;

            return _options.Provider switch
            {
                "FileSystem" => CreateFileSystemProvider(),
                "Azure" => CreateAzureProvider(),
                "Database" => CreateDatabaseProvider(),    
            };
        }

        public IBlobProviderService CreateFileSystemProvider()
        {
            var dbContext = _serviceProvider.GetRequiredService<WiseDbContext>();
            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            var basePath = configuration.GetSection("BlobStorage:FileSystem:BasePath").Value;
            basePath = Path.Combine(Directory.GetCurrentDirectory(), basePath);
            return new FileSystemBlobProviderService(basePath,dbContext);
        }
       
        private IBlobProviderService CreateDatabaseProvider()
        {
            var dbContext = _serviceProvider.GetRequiredService<WiseDbContext>();
            return new DatabaseBlobProviderService(dbContext);
        }

        private IBlobProviderService CreateAzureProvider()
        {
            //var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            //var connectionString = configuration.GetSection("BlobStorage:Azure:ConnectionString").Value;
            return new AzureBlobProviderService();
        }

    }
}
