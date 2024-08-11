using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Common
{
    public class LookUpDTO
    {
        public Guid id { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }
}
