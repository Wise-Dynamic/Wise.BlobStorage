namespace Wise.BlobStorage.Domain.Models
{
    public class BlobStorageOption
    {
        public string Provider { get; set; }
        public FileSystemOption FileSystem { get; set; }
        public AzureBlobOption AzureOption { get; set; }
        public DatabaseOption DatabaseOption { get; set; }
    }
}
