using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
   public class ConversionKey
    {
        [Key]
        public int ConversionKeyId { get; set; }
        public string ConversionKeyName { get; set; }
        public string ConversionKeyDescription { get; set; }
        public List<LanguageConversion> LanguageConversions { get; set; }

    }
}
