using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class SocialMedia
    {
        [Key]
        public int SocialMediaId { get; set; }
        public string PostUrl { get; set; }
        [MaxLength(20, ErrorMessage = "maximum girilecek değer 20 karakter")]
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public int Queno { get; set; }
        [MaxLength(100, ErrorMessage = "maximum girilecek değer 100 karakter")]
        public string SocialMediaName { get; set; }
    }
}
