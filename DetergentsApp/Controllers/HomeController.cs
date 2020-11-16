using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace DetergentsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();

        public ActionResult Index()
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

        public ActionResult Product_Read([DataSourceRequest] DataSourceRequest request)
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
        public ActionResult Products_Create_Update([DataSourceRequest] DataSourceRequest request, ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var category = db.Categories.Find(product.categoryID);
                var entity = new Product

                {
                    EAN = product.EAN,
                    title = product.title,
                    productName = product.productName,
                    productDescription = product.productDescription,
                    Category = category
                };
                product.productID = entity.productID;
                product.categoryID = entity.categoryID;
                db.Products.Add(entity);
                db.SaveChanges();
            }

            return Json(new[] {product}.ToDataSourceResult(request, ModelState));
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Destroy([DataSourceRequest] DataSourceRequest request, Product product)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product
                {
                    productID = product.productID,
                    EAN = product.EAN,
                    title = product.title,
                    productName = product.productName,
                    productDescription = product.productDescription
                };

                db.Products.Attach(entity);
                db.Products.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] {product}.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}