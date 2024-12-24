using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wise.BlobStorage.Application.Commands;
using Wise.BlobStorage.Application.Queries;

namespace Wise.BlobStorage.API.Controllers
{
    [ApiController]
    //[Authorize]
    [AllowAnonymous]
    public class BlobController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BlobController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("api/blobs")]
        public async Task<IActionResult> CreateBlob(IFormFile formFile , string? containerName = "default" )
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

        [HttpGet("api/blobs/{id}")]
        public async Task<IActionResult> GetBlob([FromRoute]long id)
        {
            var data = await _mediator.Send(new GetBlobCommand { BlobId = id });
            const string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(data, mimeType , "deneme.xlsx");
        }
    }
}
