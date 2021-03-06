﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DetergentsApp.Models
{
    public class ProductViewModel
    {
        public int productID { get; set; }
        public int categoryID { get; set; }
        public int sheetTypeID { get; set; }
        public int vendorID { get; set; }
        public int CountryID { get; set; }


        public countryViewModel Country { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Product Description")]
        public string productDescription { get; set; }

        [Required] [DisplayName("EAN")] public string EAN { get; set; }

        public string categoryName { get; set; }

        public string sheetTypeName { get; set; }

        public string vendorName { get; set; }

        public List<UserFile> listOfFiles { get; set; }

        public bool adminToPublic { get; set; }


        // Store name and ID 
        //public string name { get; set; }

        //Article name , description and id

        //  public long articleId { get; set; }

        // public string articleTextReceipt { get; set; }
        public int articleGroupId { get; set; }
        public string articleGroupDescription { get; set; }
    }
}