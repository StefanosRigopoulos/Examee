using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface ICloudinaryService
    {
        string GetExameeRendererURL();
        string GetDocumentationURL();
        Task<RawUploadResult> UploadFileAsync(Stream fileStream, string fileName);
        Task<DeletionResult> DeleteExamFileAsync(string publicId);
    }
}