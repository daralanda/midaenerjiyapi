using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class ContactInfo
    {
        [Key]
        public int ContactInfoId { get; set; }
        [MaxLength(100, ErrorMessage = "maximum girilecek değer 100 karakter")]
        public string Name { get; set; }
        [MaxLength(20, ErrorMessage = "maximum girilecek değer 20 karakter")]
        public string Phone { get; set; }
        [MaxLength(20, ErrorMessage = "maximum girilecek değer 20 karakter")]
        public string Fax { get; set; }
        [MaxLength(300, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string Email { get; set; }
        [MaxLength(400, ErrorMessage = "maximum girilecek değer 400 karakter")]
        public string Address { get; set; }
        public string MapFrame { get; set; }
        public string IsActive { get; set; }
        public bool IsDefault { get; set; }
    }
}
