using API.Errors;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseAPIController
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IUnitOfWork _uow;
        public AdminController(ICloudinaryService cloudinaryService, IUnitOfWork uow)
        {
            _cloudinaryService = cloudinaryService;
            _uow = uow;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("delete_user")]
        public async Task<IActionResult> DeleteUser([FromForm] string username) {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound(new ApiException(404, "User not found!"));

            if (user.Role.Equals("Admin")) {
                return BadRequest(new ApiException(400, "You can not delete yourself!"));
            }
            if (user.Role.Equals("Student")) {
                _uow.UserRepository.Delete(user);
                if (await _uow.Complete()) return Ok();
                return BadRequest(new ApiException(400, "Problem deleting user!"));
            }
            if (user.Role.Equals("Professor")) {
                var exams = user.Exams;
                if (exams != null) {
                    foreach (var exam in exams)
                    {
                        _uow.ExamRepository.Delete(exam);
                        user.Exams.Remove(exam);
                    }
                }
                _uow.UserRepository.Delete(user);
                if (await _uow.Complete()) return Ok();
                return BadRequest(new ApiException(400, "Problem deleting user!"));
            }
            return BadRequest(new ApiException(500, "Unexpected error!"));
        }
        
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("delete_user_exam")]
        public async Task<IActionResult> DeleteUserExam([FromForm] string username, [FromForm] string examname) {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound(new ApiException(404, "User not found!"));
            if (user.Role.Equals("Admin") || user.Role.Equals("Student")) {
                return BadRequest(new ApiException(400, "Admin and students dont have exams!"));
            }

            var exam = await _uow.ExamRepository.GetExamEntityAsync(username, examname);
            if (exam == null) return NotFound(new ApiException(404, "Exam not found!"));

            if (exam.PublicId != null)
            {
                var result = await _cloudinaryService.DeleteExamFileAsync(exam.PublicId);
                if (result.Error != null) return BadRequest(new ApiException(400, result.Error.Message));
            }

            user.Exams.Remove(exam);
            _uow.ExamRepository.Delete(exam);

            if (await _uow.Complete()) return Ok();
            return BadRequest(new ApiException(400, "Problem deleting exam file!"));
        }
    }
}