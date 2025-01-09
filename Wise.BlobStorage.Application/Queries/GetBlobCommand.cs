using BlobStorage.Interfaces;
using MediatR;
using Wise.BlobStorage.Application.Dtos;

namespace Wise.BlobStorage.Application.Queries
{
    public class GetBlobCommand : IRequest<BlobDto>
    {
        public Guid BlobId { get; set; }
    }

    public class GetBlobCommandHandler : IRequestHandler<GetBlobCommand, BlobDto>
    {
        private readonly IBlobProviderFactoryService _blobProviderFactory;
        private readonly IBlobService _blobService;
        public GetBlobCommandHandler(IBlobProviderFactoryService blobProviderFactory, IBlobService blobService)
        {
            _blobProviderFactory = blobProviderFactory;
            _blobService = blobService;
        }

        public async Task<BlobDto> Handle(GetBlobCommand request, CancellationToken cancellationToken)
        {
            var (provider,blob) = await _blobProviderFactory.GetProvider(request.BlobId);
            var data = await provider.GetAsync(blob);
            var mimeType = _blobService.GetMimeType(blob.BlobName);

            return new BlobDto() 
            { 
                Data = data , 
                FileName = blob.BlobName , 
                MimeType = mimeType 
            };
        }

       
    }
}
