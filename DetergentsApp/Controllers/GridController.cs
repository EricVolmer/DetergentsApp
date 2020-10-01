using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;

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
                validFrom = product.validFrom,
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
            var products = db.Products.Include(product => product.Category);
            var categories = db.Categories.ToList();
            ViewData["categories"] = categories;
            return View(products);
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
    }
}