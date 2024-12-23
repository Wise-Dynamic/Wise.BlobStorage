using BlobStorage.Interfaces;

namespace BlobStorage.Providers
{
    public class FileSystemBlobProvider : IBlobProvider , IFileSystemBlobProvider
    {
        private readonly string _basePath;
        public FileSystemBlobProvider(string basePath)
        {
            _basePath = basePath;
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
        }

        public async Task<byte[]> GetAsync(string containerName, string blobName)
        {
            var filePath = Path.Combine(_basePath, containerName, blobName);

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
