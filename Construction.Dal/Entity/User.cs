using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(300, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string Email { get; set; }
        [MaxLength(20, ErrorMessage = "maximum girilecek değer 20 karakter")]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public DateTime CreateDate { get; set; }
        [MaxLength(30, ErrorMessage = "maximum girilecek değer 30 karakter")]
        public string FirstName { get; set; }
        [MaxLength(30, ErrorMessage = "maximum girilecek değer 30 karakter")]
        public string LastName { get; set; }
        public virtual Role Role { get; set; }
    }
}
