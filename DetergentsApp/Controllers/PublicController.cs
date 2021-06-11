using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace DetergentsApp.Controllers
{
    public class PublicController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();

        public ActionResult Public()
        {
            CreateViewListCategory();
            CreateViewListCountry();

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

        public void CreateViewListCountry()
        {
            try
            {
                var result = db.Country;

                var containerList = new List<SelectListItem>();
                var productViewModels = result.Select(entity => new countryViewModel
                    {
                        CountryName = entity.CountryName,
                        CountryID = entity.CountryID
                    })
                    .ToList();

                foreach (var productViewModel in productViewModels)
                    containerList.Add(new SelectListItem
                        {Text = productViewModel.CountryName, Value = productViewModel.CountryID.ToString()});

                ViewBag.Country = containerList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ActionResult Product_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                // should be true not false
                var result = db.Products.Where(x => x.adminToPublic);
                var productList = new List<ProductViewModel>();
                foreach (var item in result)
                {
                    var product = new ProductViewModel
                    {
                        productID = item.productID,
                        productDescription = item.productDescription,
                        EAN = item.EAN,

                        categoryID = item.Category.categoryID,

                        CountryID = item.countryID

                        //  articleId = item.articleID
                    };


                    var fileListName = db.UserFiles
                        .Where(file => file.productID == item.productID && file.adminApproved).ToList();

                    if (fileListName != null)
                    {
                        product.listOfFiles = new List<UserFile>();

                        foreach (var file in fileListName)
                        {
                            var userFiles = new UserFile
                            {
                                fileName = file.fileName,
                                fileID = file.fileID,

                                SheetType = new SheetType
                                {
                                    sheetTypeID = file.SheetType.sheetTypeID,
                                    sheetTypeName = file.SheetType.sheetTypeName
                                }
                            };
                            userFiles.SheetType.sheetTypeName = file.SheetType.sheetTypeName;

                            product.listOfFiles.Add(userFiles);
                        }
                    }

                    productList.Add(product);
                }

                return Json(productList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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

        public ActionResult Country()
        {
            var dataContext = new DetergentsEntities();
            var country = dataContext.Country
                .Select(c => new countryViewModel
                {
                    CountryID = c.CountryID,
                    CountryName = c.CountryName
                })
                .OrderBy(e => e.CountryName);

            return Json(country, JsonRequestBehavior.AllowGet);
        }
    }
}