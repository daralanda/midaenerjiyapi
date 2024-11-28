using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Construction.Dal.Entity
{
    public class UrlRecord
    {
        [Key]
        public int UrlRecordId { get; set; }
        [MaxLength(300, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string Keywords { get; set; }
        [MaxLength(300, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string Description { get; set; }
        public string SeoUrl { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int LanguageId { get; set; }
        public int CategoryId { get; set; }
        public int SecurityObjectId { get; set; }
        public virtual Language Language { get; set; }
        public virtual Category Category { get; set; }
        public virtual SecurityObject  SecurityObject{ get; set; }
    }
}
