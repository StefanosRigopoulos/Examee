using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services {
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

            _cloudinary = new Cloudinary(acc);
        }

        public string GetDownloadURL()
        {
            return "https://res.cloudinary.com/duovczxpz/raw/upload/v1733071091/examee/BuilderTool.dll";
        }
        
        public string GetDocumentationPDFUrl()
        {
            return "https://res.cloudinary.com/duovczxpz/image/upload/v1734860810/examee/Documentation.pdf";
        }
        
        public async Task<RawUploadResult> UploadFileAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "examee/Uploaded_DLLs/"
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<DeletionResult> DeleteExamFileAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };
            return await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}