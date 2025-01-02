using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController(ICloudinaryService cloudinaryService, IUnitOfWork uow) : BaseAPIController
    {

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("delete_user")]
        public async Task<IActionResult> DeleteUser([FromForm] string username) {
            var user = await uow.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound(new ApiException(404, "User not found!", null));

            if (user.Role.Equals("Admin")) {
                return BadRequest(new ApiException(400, "Bad Request", "You can not delete yourself!"));
            }
            if (user.Role.Equals("Student")) {
                uow.UserRepository.Delete(user);
                if (await uow.Complete()) return Ok();
                return BadRequest(new ApiException(400, "Bad Request", "Problem deleting user!"));
            }
            if (user.Role.Equals("Professor")) {
                var exams = user.Exams;
                if (exams != null) {
                    foreach (var exam in exams)
                    {
                        uow.ExamRepository.Delete(exam);
                        user.Exams.Remove(exam);
                    }
                }
                uow.UserRepository.Delete(user);
                if (await uow.Complete()) return Ok();
                return BadRequest(new ApiException(400, "Bad Request", "Problem deleting user!"));
            }
            return BadRequest(new ApiException(500, "Unexpected error!", null));
        }
        
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("delete_user_exam")]
        public async Task<IActionResult> DeleteUserExam([FromForm] string username, [FromForm] string examname) {
            var user = await uow.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound(new ApiException(404, "User not found!", null));
            if (user.Role.Equals("Admin") || user.Role.Equals("Student")) {
                return BadRequest(new ApiException(400, "Bad Request", "Admin and students dont have exams!"));
            }

            var exam = await uow.ExamRepository.GetExamEntityAsync(username, examname);
            if (exam == null) return NotFound(new ApiException(404, "Exam not found!", null));

            if (exam.PublicId != null)
            {
                var result = await cloudinaryService.DeleteExamFileAsync(exam.PublicId);
                if (result.Error != null) return BadRequest(new ApiException(400, "Deletion Error", result.Error.Message));
            }

            user.Exams.Remove(exam);
            uow.ExamRepository.Delete(exam);

            if (await uow.Complete()) return Ok();
            return BadRequest(new ApiException(400, "Bad Request", "Problem deleting exam file!"));
        }
    }
}