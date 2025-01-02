using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    public class FileController(ICloudinaryService cloudinaryService, IUnitOfWork uow) : BaseAPIController
    {
        [HttpGet("get-dll")]
        public IActionResult GetDownloadUrl()
        {
            var url = cloudinaryService.GetDownloadURL();
            if (string.IsNullOrEmpty(url)) return NotFound(new ApiException(404, "DLL not found", null));
            return Ok(url);
        }

        [HttpGet("get-documentation-pdf")]
        public IActionResult GetDocumentationPDFUrl()
        {
            var url = cloudinaryService.GetDocumentationPDFUrl();
            if (string.IsNullOrEmpty(url)) return NotFound(new ApiException(404, "PDF not found", null));
            return Ok(url);
        }

        [HttpPost("upload-dll")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string username)
        {
            if (file == null || file.Length == 0) return BadRequest(new ApiException(400, "Bad Request", "File not uploaded!"));
            
            using var stream = file.OpenReadStream();
            var uploadResult = await cloudinaryService.UploadFileAsync(stream, file.FileName);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var user = await uow.UserRepository.GetUserByUsernameAsync(username);
                if (user == null) return NotFound(new ApiException(404, "User not found", null));

                var exam = new Exam {
                    ExamName = file.FileName.Replace(".dll", ""),
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.Url.ToString()
                };

                user.Exams.Add(exam);

                if (await uow.Complete()) return Ok();
                return BadRequest(new ApiException(400, "Bad Request", "Failed to add the DLL to the user's repository."));
            } else return BadRequest(new ApiException((int)uploadResult.StatusCode, "File upload failed.", null));
        }
    }
}