﻿using BlobStorage.Interfaces;
using Wise.BlobStorage.Domain.Entities;

namespace BlobStorage.Providers
{
    public class AzureBlobProviderService : IBlobProviderService , IAzureBlobProviderService
    {
        public Task DeleteAsync(string containerName, string blobName)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetAsync(Blob blob)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> SaveAsync(string containerName, string blobName, Stream data)
        {
            throw new NotImplementedException();
        }
    }
}
