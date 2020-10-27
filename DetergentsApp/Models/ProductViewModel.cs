using System.ComponentModel.DataAnnotations;

namespace DetergentsApp.Models
{
    public class ProductViewModel
    {
        [ScaffoldColumn(false)]
        public int productID { get; set; }
        public string title { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public int EAN { get; set; }
        public string categoryName { get; set; }
        
    }
}