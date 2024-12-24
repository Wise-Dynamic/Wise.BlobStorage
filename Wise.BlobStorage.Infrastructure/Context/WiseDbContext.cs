using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Wise.BlobStorage.Domain.Entities;
using Wise.BlobStorage.Infrastructure.Helpers;

namespace Wise.BlobStorage.Infrastructure.Context
{
    public class WiseDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public static readonly LoggerFactory ConsoleLoggerFactory =
            new LoggerFactory(new[] { new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider() });

        public WiseDbContext(DbContextOptions<WiseDbContext> options , IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Blob> Blobs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder == null)
                throw new ArgumentException("Options Builder is null!");

            optionsBuilder.EnableSensitiveDataLogging();

            optionsBuilder.UseLoggerFactory(ConsoleLoggerFactory);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WiseDbContext).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();

            AsyncHelper.RunSync(() => _mediator.DispatchDomainEventsAsync(this, DomainEventType.ExecuteBeforeSaving));
            var result = base.SaveChanges();
            AsyncHelper.RunSync(() => DispatchIntegrationEventsAsync(this.ChangeTracker.Entries<BaseEntity>()));
            AsyncHelper.RunSync(() => _mediator.DispatchDomainEventsAsync(this, DomainEventType.ExecuteAfterSaving));
            return result;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();

            await _mediator.DispatchDomainEventsAsync(this, DomainEventType.ExecuteBeforeSaving).ConfigureAwait(false);

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait(true);

            await DispatchIntegrationEventsAsync(this.ChangeTracker.Entries<BaseEntity>()).ConfigureAwait(false);
            await _mediator.DispatchDomainEventsAsync(this, DomainEventType.ExecuteAfterSaving).ConfigureAwait(false);

            return result;
        }

        private async Task DispatchIntegrationEventsAsync(IEnumerable<EntityEntry<BaseEntity>> entries)
        {
            var entities = entries.Where(x => x.Entity.IntegrationEvents?.Any() == true);

            var integrationEvents = entities.SelectMany(x => x.Entity.IntegrationEvents).ToList();

            entities.ToList().ForEach(x => x.Entity.ClearIntegrationEvents());
        }


        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is IEntity);
            foreach (var entiry in entries)
            {
                var unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                switch (entiry.State)
                {
                    case EntityState.Added:
                        entiry.CurrentValues[nameof(IEntity.CreatedDate)] = unixTime;
                        entiry.CurrentValues[nameof(IEntity.ModifiedDate)] = unixTime;
                        Guid guid = (Guid)entiry.CurrentValues[nameof(IEntity.Guid)];
                        if (guid == null || guid == Guid.Empty)
                        {
                            entiry.CurrentValues[nameof(IEntity.Guid)] = Guid.NewGuid();
                        }

                        break;
                    case EntityState.Modified:
                        entiry.CurrentValues[nameof(IEntity.ModifiedDate)] = unixTime;
                        entiry.Property(nameof(IEntity.CreatedDate)).IsModified = false;
                        entiry.Property(nameof(IEntity.Guid)).IsModified = false;
                        break;
                    case EntityState.Deleted:
                        entiry.Property(nameof(IEntity.CreatedDate)).IsModified = false;
                        entiry.Property(nameof(IEntity.Guid)).IsModified = false;
                        entiry.State = EntityState.Modified;
                        entiry.CurrentValues[nameof(IEntity.ModifiedDate)] = unixTime;
                        entiry.CurrentValues[nameof(IEntity.IsDeleted)] = true;
                        break;
                }
            }
        }

    }
}
