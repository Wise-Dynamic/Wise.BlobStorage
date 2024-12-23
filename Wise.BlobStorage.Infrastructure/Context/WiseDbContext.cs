using Microsoft.EntityFrameworkCore;
using Wise.BlobStorage.Domain.Entities;

namespace Wise.BlobStorage.Infrastructure.Context
{
    public class WiseDbContext : DbContext
    {
        public WiseDbContext(DbContextOptions<WiseDbContext> options) : base(options)
        {
            
        }

        public DbSet<Blob> Blobs { get; set; }
    }
}
