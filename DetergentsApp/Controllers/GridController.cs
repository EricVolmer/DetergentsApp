using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class ProductService : IDisposable
    {
        private readonly DetergentsEntities db;


        public ProductService(DetergentsEntities db)
        {
            this.db = db;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<Product> Read()
        {
            return db.Products;
        }

        public void Create(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();

            product.ProductID = product.ProductID;
        }

        public void CatCreat(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();

            category.CategoryID = category.CategoryID;
        }

        public void Update(Product product)
        {
            db.Products.Attach(product);
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Destroy(Product product)
        {
            var entity = new Product
            {
                ProductID = product.ProductID,
                EAN = product.EAN,
                Title = product.Title,
                productName = product.productName,
                productDescription = product.productDescription,
                Category = product.Category
            };

            db.Products.Attach(entity);
            db.Products.Remove(entity);
            db.SaveChanges();
        }
    }

    public class GridController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();
        private readonly ProductService productService;

        public GridController()
        {
            productService = new ProductService(new DetergentsEntities());
        }

        protected override void Dispose(bool disposing)
        {
            productService.Dispose();

            base.Dispose(disposing);
        }


        public ActionResult Grid()
        {
            var categories = db.Categories.ToList();

            ViewBag.Category = categories;
            ViewData["categories"] = categories;
            return View(new DetergentsEntities().Categories);

            // var products = db.Products.Include(product => product.Category);

            // return View(products);
        }

        public ActionResult Products_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.Products.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Create(Product product)


        {
            if (ModelState.IsValid)
            {
                productService.Create(product);


                var routeValues = this.GridRouteValues();
                return RedirectToAction("Grid", routeValues);
            }

            return View("Grid", productService.Read());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Category_Create(Category category)

        {
            if (ModelState.IsValid)
            {
                productService.CatCreat(category);

                var routeValues = this.GridRouteValues();
                return RedirectToAction("Grid", routeValues);
            }

            return View("Grid", productService.Read());
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Update(Product product)
        {
            if (ModelState.IsValid)
            {
                productService.Update(product);

                var routeValues = this.GridRouteValues();
                return RedirectToAction("Grid", routeValues);
            }

            return View("Grid", productService.Read());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Destroy(Product product)
        {
            RouteValueDictionary routeValues;

            productService.Destroy(product);

            routeValues = this.GridRouteValues();

            return RedirectToAction("Grid", routeValues);
        }


        public ActionResult FilesRead(string[] fileNames)
        {
            if (fileNames != null)
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName).ToList();
                }

            return Content("");


            // var userFiles = db.Products.Select(f => new ProductViewModel()
            // {
            //     productID = f.ProductID,
            //     productName = f.productName,
            // });
            //
            // return Json(userFiles.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FilesDestroy([DataSourceRequest] DataSourceRequest request, ProductViewModel file)
        {
            if (file != null)
            {
                db.Products.Remove(db.Products.FirstOrDefault(f => f.ProductID == file.productID));

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