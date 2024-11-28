using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class Language
    {
        [Key]
        public int LanguageId { get; set; }
        [MaxLength(5, ErrorMessage = "maximum girilecek değer 5 karakter")]
        public string ShortCode { get; set; }
        [MaxLength(30, ErrorMessage = "maximum girilecek değer 30 karakter")]
        public string LanguageName { get; set; }
        public bool IsActive { get; set; }

        [MaxLength(300, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string MetaKeywords { get; set; }
        [MaxLength(300, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string Description { get; set; }
        [MaxLength(300, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string Title { get; set; }
        public int Queno { get; set; }
        [MaxLength(10, ErrorMessage = "maximum girilecek değer 300 karakter")]
        public string UrlText { get; set; }
        public string UrlString { get; set; }
        public string ContactString { get; set; }
        public string ContactUrl { get; set; }
        public List<UrlRecord> UrlRecords { get; set; }
        public List<SliderConversion> SliderConversions { get; set; }
        public List<LanguageConversion> LanguageConversions { get; set; }
    }
}
