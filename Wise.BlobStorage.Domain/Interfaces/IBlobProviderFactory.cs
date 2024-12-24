using Wise.BlobStorage.Domain.Entities;

namespace BlobStorage.Interfaces
{
    public interface IBlobProviderFactory
    {
        IBlobProvider Create();
        Task<(IBlobProvider, Blob)> GetProvider(long blobId);
    }
}
