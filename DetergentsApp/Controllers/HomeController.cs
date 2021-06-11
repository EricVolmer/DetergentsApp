using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;

// ReSharper disable All

namespace DetergentsApp.Controllers
{
    public class HomeController : Controller
    {
#pragma warning disable 414
        private readonly string baseUrl = "https://api.sallinggroup.com/";
#pragma warning restore 414

        private readonly DetergentsEntities db = new DetergentsEntities();
        private HttpClientHandler _handler = null;

        private HttpClientHandler ClientHandler
        {
            get
            {
                if (Debugger.IsAttached)
                {
                    if (_handler == null)
                    {
                        _handler = new HttpClientHandler
                        {
                            Proxy = new WebProxy("prx.dsg.dk:8080", false)
                            {
                                Credentials = CredentialCache.DefaultNetworkCredentials
                            }
                        };
                    }
                }
                else
                {
                    _handler = new HttpClientHandler();
                }

                return _handler;
            }
        }


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

        public string generateJWTToken(string key, string issuer, string apiURL)
        {
            byte[] symmetricKey = Encoding.ASCII.GetBytes(key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;
            int expireMinutes = 5;
            var uri = new Uri(apiURL);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = issuer,
                TokenType = "JWT",
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, issuer),
                    new Claim(ClaimTypes.Uri, apiURL.ToString()),
                    new Claim("sub", uri.PathAndQuery.ToString()),
                    new Claim("mth", "GET")
                }),
                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
                SigningCredentials = new SigningCredentials(new
                        SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        [HttpGet]
        public async Task<JsonResult> FetchArticles(string ids)
        {
            string authorizationToken = "";
            string apiURL = ConfigurationManager.AppSettings["apiUrl"];
            string articleServiceUrl = "";
            string strStores = ConfigurationManager.AppSettings["Stores"];
            int[] stores;
            stores = strStores.Split(',').Select(int.Parse).ToArray();
            List<ArticleViewModel> articleList = new List<ArticleViewModel>();
            ArticleData responseItem = null;

            try
            {
                foreach (int store in stores)
                {
                    articleServiceUrl = string.Concat(apiURL,
                        string.Format("?store={0}&chain={1}&ids={2}",
                            store.ToString(),
                            "0",
                            ids));

                    authorizationToken = this.generateJWTToken(
                        Convert.ToString(ConfigurationManager.AppSettings["Secret"]),
                        Convert.ToString(ConfigurationManager.AppSettings["Issuer"]),
                        articleServiceUrl);

                    using (HttpClient client = new HttpClient(ClientHandler, false))
                    {
                        client.MaxResponseContentBufferSize = 1000000;

                        client.DefaultRequestHeaders.Add("Authorization", string.Format("JWT {0}", authorizationToken));
                        client.DefaultRequestHeaders.Add("Accept", "application/json;charset=UTF-8");

                        string args = string.Format("store={0}&chain={1}&ids={2}",
                            store.ToString(),
                            "0",
                            ids);

                        HttpResponseMessage response = await client.GetAsync(articleServiceUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            responseItem = JsonConvert.DeserializeObject<ArticleData>(
                                response.Content.ReadAsStringAsync().Result);

                            if (responseItem != null)
                            {
                                string resultInfo = string.Format("Successfully call API with args: {0}", args);
                                if (responseItem.articles.Count > 0)
                                {
                                    responseItem.articles.ForEach(article => articleList.Add(article));
                                    break;
                                }
                            }
                            else
                            {
                                string resultInfo = string.Format("Failed to call API. args: {0}", args);
                            }
                        }
                        else
                        {
                            string resultInfo = string.Format("Failed to call API. args: {0}", args);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(articleList, JsonRequestBehavior.AllowGet);
        }

#pragma warning disable 1998
        public async Task<ActionResult> Product_Read([DataSourceRequest] DataSourceRequest request)
#pragma warning restore 1998
        {
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

                        CountryID = item.countryID
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
                        // entity.articleID = product.articleId;
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
                    // articleID = product.articleId,
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
            var result = db.Country;
            var containerList = new List<SelectListItem>();
            var country = result.Select(entitiy => new countryViewModel()
                {
                    CountryID = entitiy.CountryID,
                    CountryName = entitiy.CountryName
                })
                .ToList();
            containerList.Add(new SelectListItem
                {Text = "All", Value = "0"});

            foreach (var countryViewModel in country)
                containerList.Add(new SelectListItem
                    {Text = countryViewModel.CountryName, Value = countryViewModel.CountryID.ToString()});

            ViewBag.Country = containerList;

            return Json(containerList, JsonRequestBehavior.AllowGet);
        }

        public void SignIn()
        {
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