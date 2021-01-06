using System.Collections.Generic;
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
        public string EAN { get; set; }

        public string categoryName { get; set; }
        
        public string sheetTypeName { get; set; }
        
        public string vendorName { get; set; }

        public List<UserFile> listOfFiles { get; set; }
    }
}