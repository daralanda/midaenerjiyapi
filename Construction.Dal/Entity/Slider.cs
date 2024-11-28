using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Construction.Dal.Entity
{
    public class Slider
    {
        public int SliderId { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public int Queno { get; set; }
        public List<SliderConversion> SliderConversions { get; set; }
    }
}
