using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class RolePermission
    {
        [Key]
        public int RolePermissionId { get; set; }
        public int RoleId { get; set; }
        public int SecurityObjectId { get; set; }
        public bool IsDelete { get; set; }
        public bool IsSelect { get; set; }
        public bool IsInsert { get; set; }
        public bool IsUpdate { get; set; }


        public virtual Role Role { get; set; }
        public virtual SecurityObject SecurityObject { get; set; }

    }
}
