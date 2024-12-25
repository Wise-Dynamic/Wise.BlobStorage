using BlobStorage.Interfaces;
using Wise.BlobStorage.Domain.Entities;
using Wise.BlobStorage.Infrastructure.Context;

namespace BlobStorage.Providers
{
    public class FileSystemBlobProviderService : IBlobProviderService , IFileSystemBlobProviderService
    {
        private readonly string _basePath;
        private readonly WiseDbContext _context;
        public FileSystemBlobProviderService(string basePath , WiseDbContext context)
        {
            _basePath = basePath;
            _context = context;
        }

        public async Task SaveAsync(string containerName, string blobName, Stream data)
        {
            var containerPath = Path.Combine(_basePath, containerName);
            if (!Directory.Exists(containerPath))
            {
                Directory.CreateDirectory(containerPath);
            }

            var filePath = Path.Combine(containerPath, blobName);

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await data.CopyToAsync(fileStream);
            }

            var blobEntity = new Blob
            {
                ContainerName = containerName,
                BlobName = blobName,
                BlobType = BlobType.FileSystem
            };

            await _context.Blobs.AddAsync(blobEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<byte[]> GetAsync(Blob blob)
        {
            var filePath = Path.Combine(_basePath, blob.ContainerName, blob.BlobName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Dosya bulunamadı.");
            }

            return await File.ReadAllBytesAsync(filePath);
        }

        public Task DeleteAsync(string containerName, string blobName)
        {
            var filePath = Path.Combine(_basePath, containerName, blobName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
    }
}
