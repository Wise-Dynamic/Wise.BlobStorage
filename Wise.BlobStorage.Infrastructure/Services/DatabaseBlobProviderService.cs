using BlobStorage.Interfaces;
using Microsoft.EntityFrameworkCore;
using Wise.BlobStorage.Domain.Entities;
using Wise.BlobStorage.Infrastructure.Context;

namespace BlobStorage.Providers
{
    public class DatabaseBlobProviderService : IBlobProviderService, IDatabaseBlobProviderService
    {
        private readonly WiseDbContext _context;
        public DatabaseBlobProviderService(WiseDbContext context)
        {
            _context = context;
        }

        public async Task<long> SaveAsync(string containerName, string blobName, Stream data)
        {
            using (var memoryStream = new MemoryStream())
            {
                await data.CopyToAsync(memoryStream);

                var blobEntity = new Blob
                {
                    ContainerName = containerName,
                    BlobName = blobName,
                    Data = memoryStream.ToArray(),
                    BlobType = BlobType.Database
                };

                await _context.Blobs.AddAsync(blobEntity);
                await _context.SaveChangesAsync();

                return blobEntity.Id;
            }
        }

        public async Task<byte[]> GetAsync(Blob blob)
        {
            return blob.Data;
        }


        public async Task DeleteAsync(string containerName, string blobName)
        {
            var blob = await _context.Blobs
             .FirstOrDefaultAsync(x => x.ContainerName == containerName && x.BlobName == blobName);

            if (blob != null)
            {
                _context.Blobs.Remove(blob);
                await _context.SaveChangesAsync();
            }
        }
     
    }
}
