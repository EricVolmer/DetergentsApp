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
            CreateViewListSheetType();
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

        public void CreateViewListSheetType()
        {
            try
            {
                var result = db.SheetTypes;

                var containerList = new List<SelectListItem>();
                var productViewModels = result.Select(entity => new ProductViewModel
                    {
                        sheetTypeName = entity.sheetTypeName,
                        sheetTypeID = entity.sheetTypeID
                    })
                    .ToList();

                foreach (var productViewModel in productViewModels)
                    containerList.Add(new SelectListItem
                        {Text = productViewModel.sheetTypeName, Value = productViewModel.sheetTypeID.ToString()});

                ViewBag.SheetType = containerList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ActionResult Product_Read([DataSourceRequest] DataSourceRequest request)
        {
            var fileCount = db.UserFiles.Count();

            try
            {
                var result = db.Products;
                var productList = new List<ProductViewModel>();
                foreach (var item in result)
                {
                    var product = new ProductViewModel();

                    product.productID = item.productID;
                    product.productName = item.productName;
                    product.productDescription = item.productDescription;
                    product.EAN = item.EAN;
                    product.categoryID = item.Category.categoryID;
                    product.sheetTypeID = item.sheetTypeID;
                    var fileListName = db.UserFiles.Where(file => file.productID == item.productID).ToList();
                    if (fileListName != null)
                    {
                        product.listOfFiles = new List<UserFile>();

                        foreach (var file in fileListName)
                        {
                            var userFiles = new UserFile();
                            userFiles.fileName = file.fileName;
                            userFiles.fileID = file.fileID;
                            product.listOfFiles.Add(userFiles);
                        }
                    }

                    productList.Add(product);
                }


                //var productList = result.Select(entity => new ProductViewModel
                //    {
                //        productID = entity.productID,
                //        productName = entity.productName,
                //        productDescription = entity.productDescription,
                //        EAN = entity.EAN,
                //        categoryID = entity.Category.categoryID,
                //        sheetTypeID = entity.sheetTypeID,
                //        fileCount = fileCount,
                //        listOfFiles = db.UserFiles.Where(file => file.productID == entity.productID).Select(file => new UserFile { fileName = file.fileName, fileID = file.fileID }).ToList()
                //    })
                //    .ToList();
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
    }
}