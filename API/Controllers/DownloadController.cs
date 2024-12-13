using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    public class DownloadController : BaseAPIController
    {
        private readonly ICloudinaryService _cloudinaryService;
        public DownloadController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }
        [HttpGet("get_dll")]
        public IActionResult GetDownloadUrl()
        {
            var url = _cloudinaryService.GetDownloadURL();
            if (string.IsNullOrEmpty(url)) return NotFound("File not found.");
            return Ok(url);
        }

    }
}