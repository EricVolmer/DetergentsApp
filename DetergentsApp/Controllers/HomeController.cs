using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

// ReSharper disable All

namespace DetergentsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string baseUrl = "https://api.sallinggroup.com/";

        private readonly DetergentsEntities db = new DetergentsEntities();


        public ActionResult Index()
        {
            CreateViewListCategory();
            CreateViewListSheetType();
            CreateViewListVendor();
            CreateViewListCountry();
            return View();
        }

        // There is a problem with the Authorization Token that ever time when you run the program
        // you need to copy the Token from Postman in order to get a response.


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
                var result = db.Vendor;

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

#pragma warning disable 1998
        public async Task<ActionResult> Product_Read([DataSourceRequest] DataSourceRequest request)
#pragma warning restore 1998
        {
            // responseJson = JSON.parse(responseBody);


            //var vikingStoreId = new ProductViewModel().vikingStoreId;
            //string url = baseUrl + string.Format("v2/stores/?vikingStoreId={0}", vikingStoreId);
            //var restclient = new RestClient(url);
            //restclient.Timeout = -1;


            //var request2 = new RestRequest(Method.GET);
            //request2.AddHeader("Authorization", "JWT eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MTQzNDE1MDcsImlzcyI6IjhiMmUzMmU1LWExNjYtNDdiYy05M2VkLWU4Y2Y5NDYyODc0NiIsIm10aCI6IkdFVCIsInN1YiI6Ii92Mi9zdG9yZXMvP3Zpa2luZ1N0b3JlSWQ9NTI1MSJ9.cb_qTQsMBXikhIiiOiiN4OqePU9XW3BJD87tHBVcQfc");
            //IRestResponse response = restclient.Execute(request2);
            //Console.WriteLine(response.Content);


            //  var restClient = new RestClient("https://api.sallinggroup.com/v2/stores/");
            // restClient.Timeout = -1;
            //  var request2 = new RestRequest(Method.GET);
            //  request2.AddHeader("Authorization", "JWT eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MTQ4NTgwMjQsImlzcyI6IjhiMmUzMmU1LWExNjYtNDdiYy05M2VkLWU4Y2Y5NDYyODc0NiIsIm10aCI6IkdFVCIsInN1YiI6Ii92Mi9zdG9yZXMvIn0.VsemQEdxupziO-LxK1yG3ddHMwedM066XmzA4z8-tVc");
            //  IRestResponse response = restClient.Execute(request2);
            //  Console.WriteLine(response.Content);


            // using (var client = new HttpClient())
            // {
            //     //client.BaseAddress = new Uri(baseUrl);
            //     string url = "https://api.sallinggroup.com/v2/stores/";
            //     client.DefaultRequestHeaders.Add("Authorization", "JWT eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2MTQzNDMxNDgsImlzcyI6IjhiMmUzMmU1LWExNjYtNDdiYy05M2VkLWU4Y2Y5NDYyODc0NiIsIm10aCI6IkdFVCIsInN1YiI6Ii92Mi9zdG9yZXMvIn0.ph6cGOeXzxXcBaSPbm23c5x5A12T079k7O90TvsfPyE");
            //
            //     client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //
            //     var responseTask = await client.GetAsync(url);
            //
            //     // var responseModel = JsonConvert.DeserializeObject<List<storeIDResponse>>(responseTask.Content.ToString());
            //
            //     //    responseTask.Wait();
            // }
            // //Console.WriteLine("here" + vikingStoreId);


            IEnumerable<ProductViewModel> productDescription = new List<ProductViewModel>();

            try
            {
                var result = db.Products;
                var productList = new List<ProductViewModel>();
                foreach (var item in result)
                {
                    var product = new ProductViewModel
                    {
                        productID = item.productID,
                        productDescription = item.productDescription,
                        EAN = item.EAN,

                        categoryID = item.Category.categoryID,

                        vendorID = item.Vendor.vendorID,
                        vendorName = item.Vendor.vendorName,

                        CountryID = item.countryID,

                        articleId = item.articleID
                    };


                    var fileListName = db.UserFiles
                        .Where(file => file.productID == item.productID).ToList();

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
                        entity.productDescription = product.productDescription;
                        entity.Category = category;
                        entity.vendorID = product.vendorID;
                        entity.countryID = product.CountryID;
                        entity.articleID = product.articleId;
                        entity.adminToPublic = false;

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
                    productDescription = product.productDescription,
                    Category = category,
                    SheetType = sheetTypes,
                    vendorID = product.vendorID,
                    countryID = product.CountryID,
                    articleID = product.articleId,
                    adminToPublic = false
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
            var vendor = db.Vendor.Find(product.vendorID);
            var sheetTypes = db.SheetTypes.ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new Product
                    {
                        productID = product.productID,
                        EAN = product.EAN,
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
            var country = dataContext.Country
                .Select(c => new countryViewModel
                {
                    CountryID = c.CountryID,
                    CountryName = c.CountryName
                })
                .OrderBy(e => e.CountryName);

            return Json(country, JsonRequestBehavior.AllowGet);
        }

        public void SignIn()
        {
            // Response.Cookies["ASPXPIKESADMINAUTH"].Expires = DateTime.Now.AddDays(-1);
            // Session["LoginCredentials"] = null;

            if (!Request.IsAuthenticated)
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties {RedirectUri = "/"},
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }

        /// <summary>
        ///     Send an OpenID Connect sign-out request.
        /// </summary>
        public void SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                OpenIdConnectAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}