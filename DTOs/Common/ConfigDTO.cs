using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Common
{
    public class ConfigDTO
    {
        public string SMTP { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public int Port { get; set; }
        public string CloudName { get; set; }
        public string CloudApiKey { get; set; }
        public string CloudApiSecret { get; set; }
        public string CloudFolderName { get; set; }
    }
}
