using MediatR;
using Wise.BlobStorage.Domain.Entities;
using Wise.BlobStorage.Infrastructure.Context;

namespace Wise.BlobStorage.Infrastructure.Helpers
{
    public static class MediatorExtensions
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, WiseDbContext ctx, DomainEventType domainEventType)
        {
            if (domainEventType == DomainEventType.ExecuteBeforeSaving)
            {
                var domainEntities = ctx.ChangeTracker
                    .Entries<BaseEntity>()
                    .Where(x => x.Entity.DomainEventsBeforeSaving != null && x.Entity.DomainEventsBeforeSaving.Any());

                var domainEvents = domainEntities
                    .SelectMany(x => x.Entity.DomainEventsBeforeSaving)
                    .ToList();

                domainEntities.ToList()
                    .ForEach(entity => entity.Entity.ClearDomainEvents());

                foreach (var domainEvent in domainEvents)
                    await mediator.Publish(domainEvent).ConfigureAwait(false);

            }
            else if (domainEventType == DomainEventType.ExecuteAfterSaving)
            {
                var domainEntities = ctx.ChangeTracker
                    .Entries<BaseEntity>()
                    .Where(x => x.Entity.DomainEventsAfterSaving != null && x.Entity.DomainEventsAfterSaving.Any());

                var domainEvents = domainEntities
                    .SelectMany(x => x.Entity.DomainEventsAfterSaving)
                    .ToList();

                domainEntities.ToList()
                    .ForEach(entity => entity.Entity.ClearDomainEvents());

                foreach (var domainEvent in domainEvents)
                    await mediator.Publish(domainEvent);
            }
        }
    }
}
