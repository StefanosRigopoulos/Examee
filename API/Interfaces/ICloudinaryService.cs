using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface ICloudinaryService
    {
        string GetDownloadURL();
        string GetDocumentationPDFUrl();
        Task<RawUploadResult> UploadFileAsync(Stream fileStream, string fileName);
        Task<DeletionResult> DeleteExamFileAsync(string publicId);
    }
}