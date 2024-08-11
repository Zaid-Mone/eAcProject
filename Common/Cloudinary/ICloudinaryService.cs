using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Cloudinary
{
    public interface ICloudinaryService
    {
        Task<string> SaveImage(IFormFileCollection file);
        Task<List<string>> SaveImages(IFormFileCollection file);
    }
}
