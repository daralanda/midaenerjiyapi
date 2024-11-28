using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class ContactSetting
    {

        [Key]
        public int ContactSettingId { get; set; }
        [MaxLength(100, ErrorMessage = "maximum girilecek değer 100 karakter")]
        public string Host { get; set; }
        [MaxLength(300, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string  Email { get; set; }
        [MaxLength(100, ErrorMessage = "maximum girilecek değer 100 karakter")]
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
    }
}
