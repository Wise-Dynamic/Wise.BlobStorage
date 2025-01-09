using Wise.BlobStorage.Domain.Entities;

namespace BlobStorage.Interfaces
{
    public interface IBlobProviderService
    {
        Task<Guid> SaveAsync(string containerName, string blobName , Stream data);
        Task<byte[]> GetAsync(Blob blob);
        Task DeleteAsync(string containerName, string blobName);
    }
}
