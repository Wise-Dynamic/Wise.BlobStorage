using BlobStorage.Interfaces;

namespace Wise.BlobStorage.Infrastructure.Providers
{
    public class BlobService : IBlobService
    {
        private readonly IBlobProviderFactoryService _blobProviderFactory;

        public BlobService(IBlobProviderFactoryService blobProviderFactory)
        {
            _blobProviderFactory = blobProviderFactory;
        }

        public async Task SaveBlobAsync(string containerName, string blobName, Stream data)
        {
            var provider = _blobProviderFactory.Create();
            await provider.SaveAsync(containerName, blobName, data);
        }
        
        public async Task<byte[]> GetBlobAsync(string containerName, string blobName)
        {
            var provider = _blobProviderFactory.Create();
            //return await provider.GetAsync(containerName, blobName);
            throw new NotImplementedException();
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            var provider = _blobProviderFactory.Create();
            await provider.DeleteAsync(containerName, blobName);
        }

        public string GetMimeType(string fileName)
        {
            var mimeTypes = new Dictionary<string, string>
            {
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".xls", "application/vnd.ms-excel" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".doc", "application/msword" },
                { ".pdf", "application/pdf" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".gif", "image/gif" },
                { ".txt", "text/plain" },
            };

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return mimeTypes.ContainsKey(extension) ? mimeTypes[extension] : "application/octet-stream";
        }
    }
}
