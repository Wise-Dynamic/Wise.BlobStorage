namespace BlobStorage.Interfaces
{
    public interface IBlobProvider
    {
        Task SaveAsync(string containerName, string blobName , Stream data);
        Task<byte[]> GetAsync(string containerName, string blobName);
        Task DeleteAsync(string containerName, string blobName);
    }
}
