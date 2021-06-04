using System.Collections.Generic;

namespace DetergentsApp.Models
{
    public class UserFileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int productID { get; set; }
        public int sheetTypeID { get; set; }
        
        public int CountryID { get; set; }
        public string productDescription { get; set; }
        

        public string sheetTypeName { get; set; }
        public int? vendorID { get; set; }
        public bool adminApproved { get; set; }

        public List<UserFile> listOfFiles { get; set; }


        public bool oldFile { get; set; }
        
        public string languageType { get; set; }

    }
}