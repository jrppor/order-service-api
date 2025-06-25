using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Interfaces;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderUploadController : ControllerBase
    {
        private readonly ICsvUploadService _csvUploadService;

        public OrderUploadController(ICsvUploadService csvUploadService)
        {
            _csvUploadService = csvUploadService;
        }

        [HttpPost("upload")]
        public IActionResult Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            using var stream = file.OpenReadStream();
            _csvUploadService.ProcessCsv(stream);
            return Ok("CSV uploaded and messages published.");
        }
    }
}
