using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;

namespace DetergentsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();
        private readonly string baseUrl = "https://api.sallinggroup.com/v2/stores/";


        public ActionResult Index()
        {
            CreateViewListCategory();
            CreateViewListSheetType();
            CreateViewListVendor();
            CreateViewListCountry();
            fetchAPI();
            return View();
        }

        public void fetchAPI()
        {
            
            
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

        public void CreateViewListVendor()
        {
            try
            {
                var result = db.VendorSet;

                var containerList = new List<SelectListItem>();
                var productViewModels = result.Select(entity => new ProductViewModel
                    {
                        vendorName = entity.vendorName,
                        vendorID = entity.vendorID
                    })
                    .ToList();

                foreach (var productViewModel in productViewModels)
                    containerList.Add(new SelectListItem
                    {
                        Text = productViewModel.vendorName + " - " + productViewModel.vendorID,
                        Value = productViewModel.vendorID.ToString()
                    });
                ViewBag.Vendor = containerList;
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
                var result = db.CountrySet;

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
            
            IEnumerable<ProductViewModel> ordersTotal = new List<ProductViewModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responseTask = client.GetAsync("city");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var read = result.Content.ReadAsStringAsync().Result;
                        ordersTotal = JsonConvert.DeserializeObject<IEnumerable<ProductViewModel>>(read);
                    }
                
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
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
                    product.vendorID = item.Vendor.vendorID;
                    product.vendorName = item.Vendor.vendorName;
                    product.CountryID = item.countryID;

                    var fileListName = db.UserFiles
                        .Where(file => file.productID == item.productID && file.adminApproved).ToList();

                    if (fileListName != null)
                    {
                        product.listOfFiles = new List<UserFile>();

                        foreach (var file in fileListName)
                        {
                            var userFiles = new UserFile();
                            userFiles.fileName = file.fileName;
                            userFiles.fileID = file.fileID;

                            userFiles.SheetType = new SheetType
                            {
                                sheetTypeID = file.SheetType.sheetTypeID,
                                sheetTypeName = file.SheetType.sheetTypeName
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
                        entity.vendorID = product.vendorID;
                        entity.countryID = product.CountryID;

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
                    SheetType = sheetTypes,
                    vendorID = product.vendorID,
                    countryID = product.CountryID
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
            var vendor = db.VendorSet.Find(product.vendorID);
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
                        vendorID = product.vendorID,
                        sheetTypeID = product.sheetTypeID,
                        countryID = product.CountryID,
                        Category = category,
                        Vendor = vendor,
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

        public ActionResult Country()
        {
            var dataContext = new DetergentsEntities();
            var country = dataContext.CountrySet
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