//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DetergentsApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserFile
    {
        public int fileID { get; set; }
        public string fileName { get; set; }
        public byte[] fileData { get; set; }
        public int productID { get; set; }
        public int sheetTypeID { get; set; }
        public Nullable<int> vendorID { get; set; }
        public bool adminApproved { get; set; }
        public string languageType { get; set; }
    
        public virtual SheetType SheetType { get; set; }
    }
}
