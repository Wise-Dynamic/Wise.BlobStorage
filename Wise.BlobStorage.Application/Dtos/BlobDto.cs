namespace Wise.BlobStorage.Application.Dtos
{
    public class BlobDto
    {
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public byte[] Data { get; set; }
    }
}
