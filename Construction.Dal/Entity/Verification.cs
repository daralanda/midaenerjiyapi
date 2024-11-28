using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class Verification
    {
        [Key]
        public int VerificationId { get; set; }

        [MaxLength(300, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string VerificationKey { get; set; }
        [MaxLength(300, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string VerificationValue { get; set; }
    }
}
