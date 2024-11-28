using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [MaxLength(30, ErrorMessage = "maximum girilecek değer 30 karakter")]
        public string RoleName { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public List<User> Users { get; set; }
        public List<RolePermission> RolePermissions { get; set; }

    }
}
