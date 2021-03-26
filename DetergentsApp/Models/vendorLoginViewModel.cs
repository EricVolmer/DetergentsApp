using System.ComponentModel.DataAnnotations;

namespace DetergentsApp.Models
{
    public class vendorLoginViewModel
    {
        [Required(ErrorMessage = "UserName is required")]  
        public string userName { get; set; }  
        
        [Required(ErrorMessage = "Password is required")]  
        [DataType(DataType.Password)]  
        public string password { get; set; }  
    }
}