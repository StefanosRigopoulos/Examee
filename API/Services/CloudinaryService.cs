using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services {
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);

            cloudinary = new Cloudinary(acc);
        }

        public string GetExameeToolURL()
        {
            return "https://res.cloudinary.com/duovczxpz/raw/upload/v1736247127/examee/ExameeTool.dll";
        }
        
        public string GetDocumentationURL()
        {
            return "https://res.cloudinary.com/duovczxpz/image/upload/v1736253584/examee/Documentation.pdf";
        }
        
        public async Task<RawUploadResult> UploadFileAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = "examee/Uploaded_DLLs/"
            };

            return await cloudinary.UploadAsync(uploadParams);
        }

        public async Task<DeletionResult> DeleteExamFileAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };
            return await cloudinary.DestroyAsync(deleteParams);
        }
    }
}