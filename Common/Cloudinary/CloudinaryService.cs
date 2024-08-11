using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Common.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
        private IConfiguration _config;
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;


        public CloudinaryService(IConfiguration config, CloudinaryDotNet.Cloudinary cloudinary)
        {
            _config = config;
            _cloudinary = cloudinary;
        }

        public async Task<string> SaveImage(IFormFileCollection file)
        {

            using (var stream = file[0].OpenReadStream())
            {

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file[0].FileName, stream),
                    Transformation = new Transformation().Width(1000).Height(1000),
                    Folder = _config.GetSection("CloudFolderName").Value,
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return uploadResult.Error.Message;
                }
                string imageUrl = uploadResult.SecureUrl.ToString();
                stream.Dispose();
                return imageUrl;

            }
        }
        public async Task<List<string>>SaveImages(IFormFileCollection file)
        {
            List<string> images = new List<string>();
            for (int i = 0; i < file.Count(); i++)
            {
                using (var stream = file[i].OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file[i].FileName, stream),
                        Transformation = new Transformation().Width(1000).Height(1000),
                        Folder = _config.GetSection("CloudFolderName").Value,
                    };


                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    if (uploadResult.Error != null)
                    {
                        return new List<string> { uploadResult.Error.Message };
                    }
                    string imageUrl = uploadResult.SecureUrl.ToString();
                    images.Add(imageUrl);
                }
            }
            return images;

        }
    }
}
