using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace DetergentsApp.Controllers
{
    public class GridController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();


        public ActionResult Grid()
        {
            try
            {
                var result = db.Categories;

                var containerList = new List<SelectListItem>();
                var productViewModels = result.Select(entity => new ProductViewModel
                    {
                        categoryName = entity.categoryName,
                        categoryID = entity.categoryID
                    })
                    .ToList();

                foreach (var productViewModel in productViewModels)
                    containerList.Add(new SelectListItem
                        {Text = productViewModel.categoryName, Value = productViewModel.categoryID.ToString()});

                ViewBag.Category = containerList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return View();
        }

        public ActionResult Products_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = db.Products;

                var vesselsList = result.Select(entity => new ProductViewModel
                    {
                        title = entity.title,
                        productID = entity.productID,
                        productName = entity.productName,
                        productDescription = entity.productDescription,
                        EAN = entity.EAN,
                        categoryID = entity.Category.categoryID
                    })
                    .ToList();


                return Json(vesselsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Create_Update([DataSourceRequest] DataSourceRequest request,
            ProductViewModel product)
        {
            try
            {
                var result = db.Products;

                var vesselsList = result.Select(entity => new ProductViewModel
                    {
                        productID = entity.productID,
                        productName = entity.productName,
                        productDescription = entity.productDescription,
                        EAN = entity.EAN,
                        categoryName = entity.Category.categoryName
                    })
                    .ToList();


                return Json(vesselsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Destroy(Product product)
        {
            RouteValueDictionary routeValues;

            routeValues = this.GridRouteValues();

            return RedirectToAction("Grid", routeValues);
        }


        public ActionResult FilesRead([DataSourceRequest] DataSourceRequest request)
        {
            var db = new DetergentsEntities();

            var userFiles = db.UserFiles.Select(f => new UserFileViewModel
            {
                Id = f.fileID,
                Name = f.fileName,
                DataLength = SqlFunctions.DataLength(f.fileData)
            });

            return Json(userFiles.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FilesDestroy([DataSourceRequest] DataSourceRequest request, ProductViewModel file)
        {
            if (file != null)
            {
                db.Products.Remove(db.Products.FirstOrDefault(f => f.productID == file.productID));

                db.SaveChanges();
            }

            return Json(new[] {file}.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    file.SaveAs(physicalPath);

                    // db.Products.Add(new Product()
                    // {
                    //     productName = Path.GetFileName(file.FileName),
                    // });
                }

                db.SaveChanges();
            }

            // Return an empty string to signify success
            return Content("");
        }
    }
}