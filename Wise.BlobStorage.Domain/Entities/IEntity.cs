namespace Wise.BlobStorage.Domain.Entities
{
    public interface IEntity
    {
        long Id { get; set; }
        Guid Guid { get; set; }
        string? ActionUser { get; set; }
        long? UserId { get; set; }
        bool IsDeleted { get; set; }
        long CreatedDate { get; set; }
        long? ModifiedDate { get; set; }
        DateTime RecordDate { get; set; }
    }
}
