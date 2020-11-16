﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DetergentsApp.Models
{
    public class ProductViewModel
    {
        public int productID { get; set; }

        [Required]
        [MaxLength(20)]
        [DisplayName("Safety Sheet Type")]
        public string title { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Product Name")]
        public string productName { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Product Description")]
        public string productDescription { get; set; }

        [Required]
        [DisplayName("EAN")] 
        public long EAN { get; set; }

        public string categoryName { get; set; }
        public int categoryID { get; set; }
    }
}