using Wise.BlobStorage.Domain.Entities;

namespace BlobStorage.Interfaces
{
    public interface IBlobProviderFactoryService
    {
        IBlobProviderService Create();
        Task<(IBlobProviderService, Blob)> GetProvider(long blobId);
    }
}
