using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    public class UploadController : BaseAPIController
    {
        private readonly ICloudinaryService _cloudinaryService;
        public UploadController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("dll")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("No file uploaded.");
            
            try
            {
                using var stream = file.OpenReadStream();
                var uploadResult = await _cloudinaryService.UploadFileAsync(stream, file.FileName);
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(new { Url = uploadResult.Url.ToString() });
                }
                return StatusCode((int)uploadResult.StatusCode, "File upload failed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}