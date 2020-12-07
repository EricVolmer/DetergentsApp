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
            CreateViewListCategory();
        //    CreateViewListSheetType();
            return View();
        }
        
        public void CreateViewListCategory()
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
        }
        // public void CreateViewListSheetType()
        // {
        //     try
        //     {
        //         var result = db.SheetTypes;
        //
        //         var containerList = new List<SelectListItem>();
        //         var productViewModels = result.Select(entity => new ProductViewModel
        //             {
        //                 sheetTypeName = entity.sheetTypeName,
        //                 sheetTypeID = entity.sheetTypeID
        //             })
        //             .ToList();
        //
        //         foreach (var productViewModel in productViewModels)
        //             containerList.Add(new SelectListItem
        //                 {Text = productViewModel.sheetTypeName, Value = productViewModel.sheetTypeID.ToString()});
        //
        //         ViewBag.SheetType = containerList;
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         throw;
        //     }
        // }
        public ActionResult Product_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = db.Products;
                var productList = result.Select(entity => new ProductViewModel
                    {
                        productID = entity.productID,
                        productName = entity.productName,
                        productDescription = entity.productDescription,
                        EAN = entity.EAN,
                        categoryID = entity.Category.categoryID,
                        sheetTypeID = entity.sheetTypeID
                    })
                    .ToList();
                return Json(productList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
            if (ModelState.IsValid)
            {
                var category = db.Categories.Find(product.categoryID);
                var sheetTypes = db.SheetTypes.ToList();

                if (product.productID != 0)
                {
                    var entity = db.Products.Find(product.productID);
                    if (entity != null)
                    {
                        entity.EAN = product.EAN;
                        entity.productName = product.productName;
                        entity.productDescription = product.productDescription;
                        entity.Category = category;
                        try
                        {
                            //    var existingProduct = db.Products.Find(product.productID);
                            db.Entry(entity).State = EntityState.Modified;
                            db.SaveChanges();
                            return Json(new[] {product}.ToDataSourceResult(request, ModelState));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                }

                var newProduct = new Product
                {
                    EAN = product.EAN,
                    productName = product.productName,
                    productDescription = product.productDescription,
                    Category = category,
                    SheetType = sheetTypes
                };
                try
                {
                    db.Products.Add(newProduct);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return Json(new[] {product}.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Destroy([DataSourceRequest] DataSourceRequest request, ProductViewModel product)
        {
            var category = db.Categories.Find(product.categoryID);
            var sheetTypes = db.SheetTypes.ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new Product
                    {
                        productID = product.productID,
                        EAN = product.EAN,
                        productName = product.productName,
                        productDescription = product.productDescription,
                        categoryID = product.categoryID,
                        sheetTypeID = product.sheetTypeID,
                        Category = category,
                        SheetType = sheetTypes
                    };

                    db.Products.Attach(entity);
                    db.Products.Remove(entity);
                    db.SaveChanges();
                }

                return Json(new[] {product}.ToDataSourceResult(request, ModelState));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public ActionResult sheetType_Categories()
        {
            var result = db.SheetTypes;

            var containerList = new List<SelectListItem>();
            var productViewModels = result.Select(entity => new sheetTypeViewModel
                {
                    sheetTypeName = entity.sheetTypeName,
                    sheetTypeID = entity.sheetTypeID
                })
                .ToList();
            containerList.Add(new SelectListItem
                {Text = "All", Value = "0"});

            foreach (var productViewModel in productViewModels)
                containerList.Add(new SelectListItem
                    {Text = productViewModel.sheetTypeName, Value = productViewModel.sheetTypeID.ToString()});

            ViewBag.Category = containerList;

            return Json(containerList, JsonRequestBehavior.AllowGet);
        }
    }
}