using API.Entities;
using API.Errors;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    public class FileController : BaseAPIController
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IUnitOfWork _uow;
        public FileController(ICloudinaryService cloudinaryService, IUnitOfWork uow)
        {
            _cloudinaryService = cloudinaryService;
            _uow = uow;
        }

        [HttpGet("get-dll")]
        public IActionResult GetDownloadUrl()
        {
            var url = _cloudinaryService.GetDownloadURL();
            if (string.IsNullOrEmpty(url)) return NotFound(new ApiException(404, "DLL not found."));
            return Ok(url);
        }

        [HttpGet("get-documentation-pdf")]
        public IActionResult GetDocumentationPDFUrl()
        {
            var url = _cloudinaryService.GetDocumentationPDFUrl();
            if (string.IsNullOrEmpty(url)) return NotFound(new ApiException(404, "PDF not found."));
            return Ok(url);
        }

        [HttpPost("upload-dll")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string username)
        {
            if (file == null || file.Length == 0) return BadRequest(new ApiException(400, "File not uploaded!"));
            
            using var stream = file.OpenReadStream();
            var uploadResult = await _cloudinaryService.UploadFileAsync(stream, file.FileName);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var user = await _uow.UserRepository.GetUserByUsernameAsync(username);
                if (user == null) return NotFound(new ApiException(404, "User not found!"));

                var exam = new Exam {
                    ExamName = file.FileName.Replace(".dll", ""),
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.Url.ToString()
                };

                user.Exams.Add(exam);

                if (await _uow.Complete()) return Ok();
                return BadRequest(new ApiException(400, "Failed to add the DLL to the user's repository."));
            } else return BadRequest(new ApiException((int)uploadResult.StatusCode, "File upload failed."));
        }
    }
}