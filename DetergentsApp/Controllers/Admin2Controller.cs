using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using DetergentsApp.Models;

namespace DetergentsApp.Controllers
{
    public class Admin2Controller : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();

        public ActionResult Admin2( )
        {
            

            
         //   var products = db.Products.Include(product => viewModel.categoryName);
            var categories = db.Categories.Select(c => c.CategoryName).ToList();
            ViewBag.Category = categories;
            
            ViewData["categories"] = categories;
            return View();
            
        }

        public ActionResult Products_Read([DataSourceRequest]DataSourceRequest request)
        {
            // IQueryable<Product> products = db.Products;
            // DataSourceResult result = products.ToDataSourceResult(request, product => new {
            //     ProductID = product.ProductID,
            //     EAN = product.EAN,
            //     Title = product.Title,
            //     productName = product.productName,
            //     productDescription = product.productDescription,
            //     Category = product.Category.CategoryName
            // });
            // return Json(db.Products.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);

            try
            {
                var result = db.Products;

                 var list = result.Select(entity => new ProductViewModel()
                    {
                        productID = entity.ProductID,
                        EAN = entity.EAN,
                        title = entity.Title,
                        productName = entity.productName,
                        productDescription = entity.productDescription,
                        categoryName = entity.Category.CategoryName
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
        public ActionResult Products_Create([DataSourceRequest]DataSourceRequest request, Product product)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product
                {
                    EAN = product.EAN,
                    Title = product.Title,
                    productName = product.productName,
                    productDescription = product.productDescription,
                    Category = product.Category
                };

                db.Products.Add(entity);
                db.SaveChanges();
                product.ProductID = entity.ProductID;
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Update([DataSourceRequest]DataSourceRequest request, Product product)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product
                {
                    ProductID = product.ProductID,
                    EAN = product.EAN,
                    Title = product.Title,
                    productName = product.productName,
                    productDescription = product.productDescription,
                };

                db.Products.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Destroy([DataSourceRequest]DataSourceRequest request, Product product)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product
                {
                    ProductID = product.ProductID,
                    EAN = product.EAN,
                    Title = product.Title,
                    productName = product.productName,
                    productDescription = product.productDescription,
                };

                db.Products.Attach(entity);
                db.Products.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] { product }.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
