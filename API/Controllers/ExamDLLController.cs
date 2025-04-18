using API.DTOs;
using API.Helper;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers {
    public class ExamDLLController(ICloudinaryService cloudinaryService, IUnitOfWork uow) : BaseAPIController
    {
        private readonly string exameeRendererUrl = "https://res.cloudinary.com/duovczxpz/raw/upload/v1742891500/examee/ExameeRenderer.dll";
        
        [HttpGet("{username}/")]
        public async Task<ActionResult<IEnumerable<ExamDTO>>> GetUserExams(string username)
        {
            var exams = await uow.ExamRepository.GetExamsAsync(username);
            return Ok(exams);
        }

        [HttpPost("execute_exam_file/")]
        public async Task<IActionResult> ExecuteExamFile([FromForm] IFormFile file, [FromForm] string username, [FromForm] string copies)
        {
            if (file == null || file.Length == 0) return BadRequest(new ApiException(400, "Invalid file.", "Please upload a valid DLL."));
            if (!int.TryParse(copies, out int copiesNum) || copiesNum <= 0) return BadRequest(new ApiException(400, "Invalid number of copies.", "Please provide a positive integer."));

            // Check if the exam already exists.
            var fileName = file.FileName.Replace(".dll", "");
            var exam = await uow.ExamRepository.GetExamAsync(username, fileName);
            if (exam != null) return BadRequest(new ApiException(400, "Bad Request", "Exam already exists."));

            // Create a temporary directory
            string tempDir = Path.Combine(Path.GetTempPath(), "DllProcessing_" + Guid.NewGuid());
            Directory.CreateDirectory(tempDir);
            string primaryDllPath = Path.Combine(tempDir, file.FileName);
            string dependencyDllPath = Path.Combine(tempDir, "ExameeRenderer.dll");

            CustomAssemblyLoadContext loadContext = null!;
            try {
                // Save the uploaded primary DLL
                using (var stream = new FileStream(primaryDllPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                // Download the dependency DLL from Cloudinary
                using (var httpClient = new HttpClient())
                {
                    var dependencyData = await httpClient.GetByteArrayAsync(exameeRendererUrl);
                    await System.IO.File.WriteAllBytesAsync(dependencyDllPath, dependencyData);
                }
                // Load the DLLs using a custom AssemblyLoadContext
                loadContext = new CustomAssemblyLoadContext(tempDir);
                var assembly = loadContext.LoadFromAssemblyPath(primaryDllPath);
                // Check if there is the necessary class.
                string typeName = "Exam.Program";
                var type = assembly.GetType(typeName);
                if (type == null) return BadRequest(new ApiException(400, "Structural Error", $"Class '{typeName}' not found in the assembly."));
                // Check if there is the necessary method.
                string methodName = "CreateExam";
                var method = type.GetMethod(methodName);
                if (method == null) return BadRequest(new ApiException(400, "Structural Error", $"Method '{methodName}' not found in class '{typeName}'."));
                if (method.GetParameters().Length > 0) return BadRequest(new ApiException(400, "Parameter Mismatch", $"Method '{methodName}' requires parameters but none were provided."));
                // Execute the method.
                List<string> generatedContent = new List<string>();
                for (int i = 0; i < copiesNum; i++) {
                    var instance = Activator.CreateInstance(type);
                    string buff = (string) method?.Invoke(instance, null)!;
                    if (buff != null) generatedContent.Add(buff);
                }
                // Generate a PDF using QuestPDF and return file as a response.
                if (!generatedContent.IsNullOrEmpty())
                {
                    var pdfBytes = ExamGenerator.GenerateExams(generatedContent);
                    return File(pdfBytes, "application/pdf", "Exams.pdf");
                }
                else return BadRequest(new ApiException(500, "Unexpected result type from the method.", null));
            }
            finally
            {
                if (loadContext != null)
                {
                    loadContext.Unload();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                try
                {
                    if (Directory.Exists(tempDir))
                    {
                        Directory.Delete(tempDir, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to clean up temporary files: {ex.Message}");
                }
            }
        }
    
        [HttpPost("execute_exam/")]
        public async Task<IActionResult> ExecuteExam([FromForm] string username, [FromForm] string examname, [FromForm] string copies) {
            if (!int.TryParse(copies, out int copiesNum) || copiesNum <= 0) return BadRequest(new ApiException(400, "Invalid number of copies.", "Please provide a positive integer."));

            // Get user and exam
            var user = await uow.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound(new ApiException(404, "User not found", null));

            var exam = await uow.ExamRepository.GetExamEntityAsync(username, examname);
            if (exam == null) return NotFound(new ApiException(404, "Exam not found", null));

            // Create a temporary directory
            string tempDir = Path.Combine(Path.GetTempPath(), "DllProcessing_" + Guid.NewGuid());
            Directory.CreateDirectory(tempDir);
            string primaryDllPath = Path.Combine(tempDir, exam.ExamName + ".dll");
            string dependencyDllPath = Path.Combine(tempDir, "ExameeRenderer.dll");

            CustomAssemblyLoadContext loadContext = null!;
            try {
                // Download the exam DLL from Cloudinary
                using (var httpClient = new HttpClient())
                {
                    var primaryData = await httpClient.GetByteArrayAsync(exam.Url);
                    await System.IO.File.WriteAllBytesAsync(primaryDllPath, primaryData);
                }
                // Download the dependency DLL from Cloudinary
                using (var httpClient = new HttpClient())
                {
                    var dependencyData = await httpClient.GetByteArrayAsync(exameeRendererUrl);
                    await System.IO.File.WriteAllBytesAsync(dependencyDllPath, dependencyData);
                }
                // Load the DLLs using a custom AssemblyLoadContext
                loadContext = new CustomAssemblyLoadContext(tempDir);
                var assembly = loadContext.LoadFromAssemblyPath(primaryDllPath);
                // Check if there is the necessary class.
                string typeName = "Exam.Program";
                var type = assembly.GetType(typeName);
                if (type == null) return BadRequest(new ApiException(400, "Structural Error", $"Class '{typeName}' not found in the assembly."));
                // Check if there is the necessary method.
                string methodName = "CreateExam";
                var method = type.GetMethod(methodName);
                if (method == null) return BadRequest(new ApiException(400, "Structural Error", $"Method '{methodName}' not found in class '{typeName}'."));
                if (method.GetParameters().Length > 0) return BadRequest(new ApiException(400, "Parameter Mismatch", $"Method '{methodName}' requires parameters but none were provided."));
                // Execute the method.
                List<string> generatedContent = new List<string>();
                for (int i = 0; i < copiesNum; i++) {
                    var instance = Activator.CreateInstance(type);
                    string buff = (string) method?.Invoke(instance, null)!;
                    if (buff != null) generatedContent.Add(buff);
                }
                // Generate a PDF using QuestPDF and return file as a response.
                if (!generatedContent.IsNullOrEmpty())
                {
                    var pdfBytes = ExamGenerator.GenerateExams(generatedContent);
                    return File(pdfBytes, "application/pdf", "Exams.pdf");
                }
                else return BadRequest(new ApiException(500, "Unexpected result type from the method.", null));
            }
            finally
            {
                // Ensure the custom assembly context is unloaded
                if (loadContext != null)
                {
                    loadContext.Unload();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                // Clean up temporary files
                try
                {
                    if (Directory.Exists(tempDir))
                    {
                        Directory.Delete(tempDir, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to clean up temporary files: {ex.Message}");
                }
            }
        }
    
        [HttpPost("delete_exam_file/")]
        public async Task<IActionResult> DeleteExamFile([FromForm] string username, [FromForm] string examname) {
            var user = await uow.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound(new ApiException(404, "User not found", null));
            var exam = await uow.ExamRepository.GetExamEntityAsync(username, examname);
            if (exam == null) return NotFound(new ApiException(404, "Exam not found", null));
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
