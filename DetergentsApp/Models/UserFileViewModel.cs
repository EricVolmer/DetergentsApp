namespace DetergentsApp.Models
{
    public class UserFileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DataLength { get; set; }

        public int? vendorID { get; set; }
        public int sheetTypeID { get; set; }
        public int productID { get; set; }
    }
}