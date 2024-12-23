namespace Wise.BlobStorage.Domain.Entities
{
    public class Blob
    {
        public int Id { get; set; }
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
        public byte[] Data { get; set; }
    }
}
