namespace Wise.BlobStorage.Domain.Entities
{
    public class Blob : BaseEntity
    {
        public string? ContainerName { get; set; }
        public string? BlobName { get; set; }
        public byte[]? Data { get; set; }
        public BlobType BlobType { get; set; }
    }
    public enum BlobType
    {
        FileSystem = 1,
        Database = 2,
        Azure = 3
    }
}
