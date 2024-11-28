using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class SliderConversion
    {
        [Key]
        public int SliderConversionId { get; set; }
        public int SliderId { get; set; }
        [MaxLength(100, ErrorMessage = "maximum girilecek değer 100 karakter")]
        public string Title { get; set; }
        [MaxLength(200, ErrorMessage = "maximum girilecek değer 200 karakter")]
        public string Description { get; set; }
        public string PostUrl { get; set; }
        public int LanguageId { get; set; }
        public virtual Slider Slider { get; set; }
        public virtual Language Language { get; set; }

    }
}
