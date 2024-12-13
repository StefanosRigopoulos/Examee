using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    public class ExecuteController : BaseAPIController
    {
        [HttpPost("dll")]
        public async Task<IActionResult> ExecuteDll([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Invalid file. Please upload a valid DLL.");

            string tempFilePath = Path.GetTempFileName();
            try
            {
                // Save the uploaded DLL to a temporary file
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Load the DLL using Reflection
                var assembly = Assembly.LoadFrom(tempFilePath);

                // Replace with the actual namespace and class name expected in the DLL
                string typeName = "Exam.Program";
                var type = assembly.GetType(typeName);
                if (type == null) return BadRequest($"Class '{typeName}' not found in the assembly.");

                // Replace with the actual method name expected in the DLL
                string methodName = "CreateExam";
                var method = type.GetMethod(methodName);
                if (method == null) return BadRequest($"Method '{methodName}' not found in the class '{typeName}'.");

                // Create an instance of the class
                var instance = Activator.CreateInstance(type);

                // Example parameter for the method (modify as needed)
                string examParameters = "Sample Parameters";

                // Invoke the method and get the result
                var result = method.Invoke(instance, new object[] { examParameters });

                if (result is string generatedContent)
                {
                    // Return the generated content as plain text for simplicity
                    return Ok(new { content = generatedContent });
                    // Return a PDF with the exams.
                }
                else
                {
                    return StatusCode(500, "Unexpected result type from the method.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing the DLL: {ex.Message}");
            }
            finally
            {
                if (System.IO.File.Exists(tempFilePath)) System.IO.File.Delete(tempFilePath);
            }
        }
    }
}
