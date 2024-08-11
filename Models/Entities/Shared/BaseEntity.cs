using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.Shared
{
    public abstract class BaseEntity
    {
        public long ID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
