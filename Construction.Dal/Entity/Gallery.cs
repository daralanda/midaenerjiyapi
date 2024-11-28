using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Construction.Dal.Entity
{
    public class Gallery
    {
        [Key]
        public int GalleryId { get; set; }
        public string Url { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Categories { get; set; }
    }
}
