using System;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.UI;

namespace DetergentsApp.Controllers
{
    public class AddVendorController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();

        public ActionResult AddVendor()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VendorCreate([DataSourceRequest] DataSourceRequest request,
            VendorViewModel vendor)
        {
            var newVendor = new VendorSet
            {
                vendorName = vendor.vendorName,
                vendorID = vendor.vendorID
            };
            try
            {
                db.Vendor.Add(newVendor);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return View("AddVendor");
        }
    }
}