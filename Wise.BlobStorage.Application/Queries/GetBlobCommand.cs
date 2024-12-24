using BlobStorage.Interfaces;
using MediatR;

namespace Wise.BlobStorage.Application.Queries
{
    public class GetBlobCommand : IRequest<byte[]>
    {
        public long BlobId { get; set; }
    }

    public class GetBlobCommandHandler : IRequestHandler<GetBlobCommand, byte[]>
    {
        private readonly IBlobProviderFactory _blobProviderFactory;
        public GetBlobCommandHandler(IBlobProviderFactory blobProviderFactory)
        {
            _blobProviderFactory = blobProviderFactory;
        }

        public async Task<byte[]> Handle(GetBlobCommand request, CancellationToken cancellationToken)
        {
            var (provider,blob) = await _blobProviderFactory.GetProvider(request.BlobId);
            var data = await provider.GetAsync(blob);
            return data;
        }
    }
}
