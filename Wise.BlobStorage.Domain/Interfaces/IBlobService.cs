namespace BlobStorage.Interfaces
{
    public interface IBlobService
    {
        Task SaveBlobAsync(string containerName, string blobName, Stream data);
        Task<byte[]> GetBlobAsync(string containerName, string blobName);
        Task DeleteBlobAsync(string containerName, string blobName);
    }
}
