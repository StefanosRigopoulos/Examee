using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface ICloudinaryService
    {
        string GetDownloadURL();
        Task<RawUploadResult> UploadFileAsync(Stream fileStream, string fileName);
    }
}