using ERPProject.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPProject.Entity.Base
{
    public class BaseEntity:IEntity
    {
        public DateTime? AddedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public string? AddedIPV4Address { get; set; }

        public string? UpdatedIPV4Address { get; set; }

        public long? AddedUser { get; set; }

        public long? UpdatedUser { get; set; }
        public bool? IsActive { get; set; }
       
       
    }
}
