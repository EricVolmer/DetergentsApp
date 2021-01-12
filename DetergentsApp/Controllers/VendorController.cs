using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DetergentsApp.Models;

namespace DetergentsApp.Controllers
{
    public class VendorController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();

        // GET: Vendor
        public ActionResult Index()
        {
            return View(db.VendorSet.ToList());
        }

        public ActionResult VendorSave(IEnumerable<HttpPostedFileBase> files, int vendorDetails, int articleEAN,
            int sheetTypeCategory, string language)
        {
            // var product = db.Products.SingleOrDefault(x => x.EAN == articleEAN);
            try
            {
                if (files != null)
                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var physicalPath = Path.Combine(Server.MapPath("~/App_Data/Vendor"), fileName);

                        db.UserFiles.Add(new UserFile
                        {
                            fileName = fileName,
                            fileData = GetFilesBytes(file),
                            productID = articleEAN,
                            sheetTypeID = sheetTypeCategory,
                            vendorID = vendorDetails,
                            adminApproved = false,
                            languageType = language

                            //language
                        });
                        db.SaveChanges();
                        file.SaveAs(physicalPath);
                    }

                // Return an empty string to signify success
                return View("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ActionResult vendorEANDropDownList()
        {
            var result = db.Products;

            var containerList = new List<SelectListItem>();
            var productViewModels = result.Select(entity => new ProductViewModel
                {
                    productID = entity.productID,
                    EAN = entity.EAN,
                    productName = entity.productName
                })
                .ToList();

            foreach (var productViewModel in productViewModels)
                containerList.Add(new SelectListItem
                {
                    Text = productViewModel.productName + " - " + productViewModel.EAN,
                    Value = productViewModel.productID.ToString()
                });

            ViewBag.Category = containerList;

            return Json(containerList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult vendorDropDownList()
        {
            var result = db.VendorSet;

            var containerList = new List<SelectListItem>();
            var productViewModels = result.Select(entity => new VendorViewModel
                {
                    vendorID = entity.vendorID,
                    vendorName = entity.vendorName
                })
                .ToList();

            foreach (var productViewModel in productViewModels)
                containerList.Add(new SelectListItem
                {
                    Text = productViewModel.vendorName + " - " + productViewModel.vendorID,
                    Value = productViewModel.vendorID.ToString()
                });

            ViewBag.Category = containerList;

            return Json(containerList, JsonRequestBehavior.AllowGet);
        }

        public static byte[] GetFilesBytes(HttpPostedFileBase file)
        {
            var target = new MemoryStream();

            file.InputStream.CopyTo(target);

            return target.ToArray();
        }
    }
}