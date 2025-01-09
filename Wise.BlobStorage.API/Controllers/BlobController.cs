using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wise.BlobStorage.Application.Commands;
using Wise.BlobStorage.Application.Queries;

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

        
        [HttpGet]
        [Route("show/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PreviewAsync([FromRoute]Guid id)
        {
            var result = await _mediator.Send(new GetBlobCommand { BlobId = id });

            
            var contentType = GetContentType(result.FileName);
            
            return File(result.Data,contentType);
        }
        
                
        [HttpGet]
        [Route("download/{id}")]
        public async Task<IActionResult> DownloadAsync([FromRoute]Guid id)
        {
            var result = await _mediator.Send(new GetBlobCommand { BlobId = id });
            
            return File(result.Data,"application/octet-stream", result.FileName);
        }
        
        [HttpPost("api/blobs")]
        public async Task<IActionResult> CreateBlob(IFormFile formFile , [FromQuery]string? containerName = "default" )
        {
            await using var stream = formFile.OpenReadStream();
            
            return Ok(await _mediator.Send(new CreateBlobCommand
            {
                ContainerName = containerName,
                BlobName = formFile.FileName,
                Data = stream
            }));
        }
        
        
        
        
        private string GetContentType(string path)
        {
            var types = new Dictionary<string, string>
            {
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".gif", "image/gif" },
                { ".bmp", "image/bmp" },
                { ".tiff", "image/tiff" },
                { ".ico", "image/x-icon" },
                { ".svg", "image/svg+xml" },
                { ".webp", "image/webp" }
            };

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }
      
    }
}
