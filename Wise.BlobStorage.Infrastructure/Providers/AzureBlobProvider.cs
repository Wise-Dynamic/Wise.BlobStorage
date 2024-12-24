using BlobStorage.Interfaces;
using Wise.BlobStorage.Domain.Entities;

namespace BlobStorage.Providers
{
    public class AzureBlobProvider : IBlobProvider , IAzureBlobProvider
    {
        public Task DeleteAsync(string containerName, string blobName)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetAsync(Blob blob)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(string containerName, string blobName, Stream data)
        {
            throw new NotImplementedException();
        }
    }
}
