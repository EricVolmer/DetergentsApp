using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using Kendo.Mvc.Extensions;
using DetergentsApp.Models;

namespace DetergentsApp.Controllers
{
    public class ProductService : IDisposable
    {
        private DetergentsEntities db;

        public ProductService(DetergentsEntities db)
        {
            this.db = db;
        }

        public IEnumerable<Product> Read()
        {
            return db.Products;  
        }

        public void Create(Product product)
        {
            var entity = new Product
            {
                EAN = product.EAN,
                Title = product.Title,
                productName = product.productName,
                productDescription = product.productDescription,
                validFrom = product.validFrom,
                Category = product.Category
            };

            db.Products.Add(entity);
            db.SaveChanges();

            product.ProductID = entity.ProductID;
        }

        public void Update(Product product)
        {
            var entity = new Product
            {
                ProductID = product.ProductID,
                EAN = product.EAN,
                Title = product.Title,
                productName = product.productName,
                productDescription = product.productDescription,
                validFrom = product.validFrom,
                Category = product.Category
            };

            db.Products.Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
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
                validFrom = product.validFrom,
                Category = product.Category
            };

            db.Products.Attach(entity);
            db.Products.Remove(entity);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }

    public class GridController : Controller
    {
        private ProductService productService;

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
            return View(productService.Read());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Create(Product product)
        {
            if (ModelState.IsValid)
            {
                productService.Create(product);

                RouteValueDictionary routeValues = this.GridRouteValues();
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

                RouteValueDictionary routeValues = this.GridRouteValues();
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
    }
}
