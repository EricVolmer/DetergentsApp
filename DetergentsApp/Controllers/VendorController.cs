using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace DetergentsApp.Controllers
{
    public class VendorController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();
        public ActionResult Index()
        {
            return View(db.Vendor.ToList());
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
                        var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                        db.UserFiles.Add(new UserFile
                        {
                            fileName = fileName,
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
                    productDescription = entity.productDescription,
                    EAN = entity.EAN
                })
                .ToList();

            foreach (var productViewModel in productViewModels)
                containerList.Add(new SelectListItem
                {
                    Text = productViewModel.productDescription + " - " + productViewModel.EAN,
                    Value = productViewModel.productID.ToString()
                });

            ViewBag.Category = containerList;

            return Json(containerList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult vendorDropDownList()
        {
            var result = db.Vendor;

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
        
        

        // public ActionResult vendorLogin([DataSourceRequest] DataSourceRequest request ,string username, vendorLoginViewModel password )
        // {
        //     if (ModelState.IsValid)
        //     {
        //         // try
        //         // {
        //         //     var result = db.vendorLogin;
        //         //
        //         //     var list = result.Select(entity => new vendorLoginViewModel()
        //         //         {
        //         //             userName = entity.userName,
        //         //             password = entity.password
        //         //         })
        //         //         .ToList();
        //         //     return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        //         // }
        //         // catch (Exception e)
        //         // {
        //         //     Console.WriteLine(e);
        //         //     throw;
        //         // }
        //
        //
        //         var loginPassword = db.vendorLogin.Select(entity => new vendorLoginViewModel()
        //             {
        //                 userName = entity.userName,
        //                 password = entity.password
        //             }).ToString();
        //         var f_password = GetMD5(loginPassword);
        //         var data =db.vendorLogin.Where(s => s.userName.Equals(username) && s.password.Equals(f_password)).ToList();
        //         
        //     }
        //     return View();
        // }
        //
        //
        // //Logout
        // public ActionResult Logout()
        // {
        //     Session.Clear();//remove session
        //     return RedirectToAction("Index");
        // }
        //
        // public static string GetMD5(string str)
        // {
        //     MD5 md5 = new MD5CryptoServiceProvider();
        //     byte[] fromData = Encoding.UTF8.GetBytes(str);
        //     byte[] targetData = md5.ComputeHash(fromData);
        //     string byte2String = null;
        //
        //     for (int i = 0; i < targetData.Length; i++)
        //     {
        //         byte2String += targetData[i].ToString("x2");
        //
        //     }
        //     return byte2String;
        // }
        
        public ActionResult VendorEdit()
        {
            return View(db.Vendor.ToList());
        }
        
        public ActionResult VendorRead([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = db.Vendor;

                var list = result.Select(entity => new VendorViewModel()
                    {
                        vendorID = entity.vendorID,
                        vendorName = entity.vendorName
                        
                    })
                    .ToList();
                return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }        
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VendorUpdate([DataSourceRequest] DataSourceRequest request, Vendor vendor)
        {
            if (ModelState.IsValid)
            {

                var entity = db.Vendor.Find(vendor.vendorID);
                if (entity != null)
                {
                    entity.vendorID = vendor.vendorID;
                    entity.vendorName = vendor.vendorName;

                    try
                    {
                        db.Entry(entity).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new[] {vendor}.ToDataSourceResult(request, ModelState));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            return Json(new[] {vendor}.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult VendorDestroy([DataSourceRequest] DataSourceRequest request, Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                var entity = new Vendor()
                {
                    vendorID = vendor.vendorID,
                    vendorName = vendor.vendorName
                };

                db.Vendor.Attach(entity);
                db.Vendor.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] {vendor}.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}