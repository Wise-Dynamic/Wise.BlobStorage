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
    public class BlobProviderFactory : IBlobProviderFactory
    {
        private readonly BlobStorageOption _options;
        private readonly IServiceProvider _serviceProvider;
        private readonly WiseDbContext _context;

        public BlobProviderFactory(IOptions<BlobStorageOption> options, IServiceProvider serviceProvider, WiseDbContext context)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
            _context = context;
        }

        public async Task<(IBlobProvider, Blob)> GetProvider(long blobId)
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
            var dbContext = _serviceProvider.GetRequiredService<WiseDbContext>();
            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            var basePath = configuration.GetSection("BlobStorage:FileSystem:BasePath").Value;
            basePath = Path.Combine(Directory.GetCurrentDirectory(), basePath);
            return new FileSystemBlobProvider(basePath,dbContext);
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
