using BlobStorage.Interfaces;

namespace BlobStorage.Providers
{
    public class AzureBlobProvider : IBlobProvider , IAzureBlobProvider
    {
        public Task DeleteAsync(string containerName, string blobName)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetAsync(string containerName, string blobName)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(string containerName, string blobName, Stream data)
        {
            throw new NotImplementedException();
        }
    }
}
