using BlobStorage.Interfaces;

namespace Wise.BlobStorage.Infrastructure.Providers
{
    public class BlobService : IBlobService
    {
        private readonly IBlobProviderFactory _blobProviderFactory;

        public BlobService(IBlobProviderFactory blobProviderFactory)
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
            return await provider.GetAsync(containerName, blobName);
        }

        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            var provider = _blobProviderFactory.Create();
            await provider.DeleteAsync(containerName, blobName);
        }

    }
}
