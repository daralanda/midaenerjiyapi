using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class LanguageConversion
    {
        [Key]
        public int LanguageConversionId { get; set; }
        public int LanguageId { get; set; }
        public int ConversionKeyId { get; set; }
        public string Value { get; set; }
        public virtual Language Language { get; set; }
        public virtual ConversionKey ConversionKey { get; set; }
    }
}
