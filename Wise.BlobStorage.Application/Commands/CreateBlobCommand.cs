using BlobStorage.Interfaces;
using MediatR;

namespace Wise.BlobStorage.Application.Commands
{
    public class CreateBlobCommand : IRequest<Guid>
    {
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
        public Stream Data { get; set; } 
    }
    public class CreateBlobCommandHandler : IRequestHandler<CreateBlobCommand, Guid>
    {
        private readonly IBlobProviderFactoryService _blobProviderFactory;
        
        public CreateBlobCommandHandler(IBlobProviderFactoryService blobProviderFactory)
        {
            _blobProviderFactory = blobProviderFactory;
        }

        public async Task<Guid> Handle(CreateBlobCommand request, CancellationToken cancellationToken)
        {
            var provider = _blobProviderFactory.Create();
            var blobId =  await provider.SaveAsync(request.ContainerName, request.BlobName, request.Data);
            return blobId;
        }
    }
}
