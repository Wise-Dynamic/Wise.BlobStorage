using BlobStorage.Interfaces;
using MediatR;

namespace Wise.BlobStorage.Application.Commands
{
    public class CreateBlobCommand : IRequest<bool>
    {
        public string ContainerName { get; set; }
        public string BlobName { get; set; }
        public Stream Data { get; set; } 
    }
    public class CreateBlobCommandHandler : IRequestHandler<CreateBlobCommand, bool>
    {
        private readonly IBlobProviderFactory _blobProviderFactory;
        
        public CreateBlobCommandHandler(IBlobProviderFactory blobProviderFactory)
        {
            _blobProviderFactory = blobProviderFactory;
        }

        public async Task<bool> Handle(CreateBlobCommand request, CancellationToken cancellationToken)
        {
            var provider = _blobProviderFactory.Create();
            await provider.SaveAsync(request.ContainerName, request.BlobName, request.Data);
            return true;
        }
    }
}
