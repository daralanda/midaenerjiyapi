using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Construction.Dal.Entity
{
	public class Category
    {
        [Key]
		public int CategoryId { get; set; }
		public string ImageUrl { get; set; }
		public int MainCategoryId { get; set; }
		public int CategoryType { get; set; }
		/// <summary>
		/// 1 Normal Kategori
		/// 2 Proje Liste
		/// 3 Hizmetler
		/// 4 Blog Kategori
		/// 5 Proje Detay
		/// 6 Yazı Detay
		/// 7 Hizmet Detay
		/// </summary>
		public int Queno { get; set; }
		public string Title { get; set; }
		public bool IsBlog { get; set; }
        public string Location { get; set; }
        public string Meters { get; set; }
        public string ProjectDate { get; set; }
        public List<UrlRecord> UrlRecords { get; set; }
		public List<Gallery> Galleries { get; set; }

	}
}
