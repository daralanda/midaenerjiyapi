using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class SecurityObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SecurityObjectId { get; set; }
        public int MainSecurityObjectId { get; set; }
        [MaxLength(100, ErrorMessage = "maximum girilecek değer 100 karakter")]
        public string SecurityObjectName { get; set; }
        [MaxLength(255, ErrorMessage = "maximum girilecek değer 255 karakter")]
        public string SeoUrl { get; set; }
        [MaxLength(25, ErrorMessage = "maximum girilecek değer 25 karakter")]
        public string ControllerName { get; set; }
        [MaxLength(25, ErrorMessage = "maximum girilecek değer 25 karakter")]
        public string ActionName { get; set; }
        public bool IsFront { get; set; }
        [MaxLength(25, ErrorMessage = "maximum girilecek değer 25 karakter")]
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public int Queno { get; set; }
        public List<UrlRecord> UrlRecords { get; set; }
        public List<RolePermission> RolePermissions { get; set; }

    }
}
