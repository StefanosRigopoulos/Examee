using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface ICloudinaryService
    {
        string GetExameeToolURL();
        string GetDocumentationURL();
        Task<RawUploadResult> UploadFileAsync(Stream fileStream, string fileName);
        Task<DeletionResult> DeleteExamFileAsync(string publicId);
    }
}