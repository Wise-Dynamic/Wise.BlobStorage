using MediatR;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Wise.BlobStorage.Domain.Entities
{
    public class BaseEntity : IEntity
    {
        public long Id { get ; set ; }
        public Guid Guid { get ; set ; }
        public string? ActionUser { get ; set ; }
        public long? UserId { get ; set ; }
        public bool IsDeleted { get ; set ; }
        public long CreatedDate { get ; set ; }
        public long? ModifiedDate { get ; set ; }

        public DateTime RecordDate { get; set; }
        
        public BaseEntity()
        {
            CreatedDate = long.Parse("0");
            ModifiedDate = long.Parse("0");
            IsDeleted = false;
            RecordDate = DateTime.Now;
            Guid = new Guid();
            ActionUser = "Goksel";
        }


        [NotMapped]
        [JsonIgnore] 
        private List<INotification> _domainEventsBeforeSaving;

        [JsonIgnore]
        [NotMapped]
        public IReadOnlyCollection<INotification> DomainEventsBeforeSaving => _domainEventsBeforeSaving?.AsReadOnly();


        [NotMapped]
        [JsonIgnore] 
        private List<INotification> _domainEventsAfterSaving;

        [JsonIgnore]
        [NotMapped]
        public IReadOnlyCollection<INotification> DomainEventsAfterSaving => _domainEventsAfterSaving?.AsReadOnly();


        [JsonIgnore]
        [NotMapped] 
        private List<IntegrationEvent> _integrationEvents;

        [NotMapped]
        [JsonIgnore]
        public IReadOnlyCollection<IntegrationEvent> IntegrationEvents => _integrationEvents?.AsReadOnly();


        public void AddIntegrationEvent(IntegrationEvent eventItem)
        {
            _integrationEvents ??= new List<IntegrationEvent>();
            _integrationEvents.Add(eventItem);
        }

        public void AddDomainEvent(INotification eventItem, DomainEventType type = DomainEventType.ExecuteBeforeSaving)
        {
            if (type == DomainEventType.ExecuteBeforeSaving)
            {
                _domainEventsBeforeSaving ??= new List<INotification>();
                _domainEventsBeforeSaving.Add(eventItem);
            }
            else if (type == DomainEventType.ExecuteAfterSaving)
            {
                _domainEventsAfterSaving ??= new List<INotification>();
                _domainEventsAfterSaving.Add(eventItem);
            }
        }

        public void RemoveDomainEvent(INotification eventItem)
        {
            try
            {
                _domainEventsBeforeSaving?.Remove(eventItem);
            }
            catch (Exception e)
            {
            }

            try
            {
                _domainEventsAfterSaving?.Remove(eventItem);
            }
            catch (Exception e)
            {
            }
        }

        public void ClearDomainEvents()
        {
            _domainEventsBeforeSaving?.Clear();
            _domainEventsAfterSaving?.Clear();
        }

        public void ClearIntegrationEvents()
        {
            _integrationEvents?.Clear();
        }

    }

    public enum DomainEventType
    {
        ExecuteBeforeSaving,
        ExecuteAfterSaving
    }
}
