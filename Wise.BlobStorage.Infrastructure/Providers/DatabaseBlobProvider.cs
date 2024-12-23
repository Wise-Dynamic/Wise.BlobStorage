﻿using BlobStorage.Interfaces;
using Microsoft.EntityFrameworkCore;
using Wise.BlobStorage.Domain.Entities;
using Wise.BlobStorage.Infrastructure.Context;

namespace BlobStorage.Providers
{
    public class DatabaseBlobProvider : IBlobProvider, IDatabaseBlobProvider
    {
        private readonly WiseDbContext _context;
        public DatabaseBlobProvider(WiseDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(string containerName, string blobName, Stream data)
        {
            using (var memoryStream = new MemoryStream())
            {
                await data.CopyToAsync(memoryStream);

                var blobEntity = new Blob
                {
                    ContainerName = containerName,
                    BlobName = blobName,
                    Data = memoryStream.ToArray()
                };

                await _context.Blobs.AddAsync(blobEntity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<byte[]> GetAsync(string containerName, string blobName)
        {
            var blob = await _context.Blobs
                .FirstOrDefaultAsync(x => x.ContainerName == containerName && x.BlobName == blobName);

            if (blob == null)
            {
                throw new Exception("Blob not found");
            }

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