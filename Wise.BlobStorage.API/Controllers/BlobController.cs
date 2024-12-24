using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wise.BlobStorage.Application.Commands;

namespace Wise.BlobStorage.API.Controllers
{
    [ApiController]
    [Authorize]
    public class BlobController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BlobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("api/blobs")]
        [HttpPost]
        public async Task<IActionResult> CreateBlob(string containerName , IFormFile formFile)
        {
            using (var stream = formFile.OpenReadStream())
            {
                return Ok(await _mediator.Send(new CreateBlobCommand
                {
                    ContainerName = containerName,
                    BlobName = formFile.FileName,
                    Data = stream
                }));
            }          
        }
    }
}
