using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

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
    
        
    
     public ProductService(DetergentsEntities entities)
        {
            this.entities = entities;
        }

        public IList<ProductViewModel> GetAll()
        {
            var result = HttpContext.Current.Session["Products"] as IList<ProductViewModel>;

            if (result == null || UpdateDatabase)
            {
                result = entities.Products.Select(product => new ProductViewModel
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice.HasValue ? product.UnitPrice.Value : default(decimal),
                    UnitsInStock = product.UnitsInStock.HasValue ? product.UnitsInStock.Value : default(short),
                    QuantityPerUnit = product.QuantityPerUnit,
                    Discontinued = product.Discontinued,
                    UnitsOnOrder = product.UnitsOnOrder.HasValue ? (int)product.UnitsOnOrder.Value : default(int),
                    CategoryID = product.CategoryID,
                    Category = new CategoryViewModel()
                    {
                        CategoryID = product.Category.CategoryID,
                        CategoryName = product.Category.CategoryName
                    },
                    LastSupply = DateTime.Today
                }).ToList();

                HttpContext.Current.Session["Products"] = result;
            }

            return result;
        }

        public IEnumerable<ProductViewModel> Read()
        {
            return GetAll();
        }

        public void Create(ProductViewModel product)
        {
            if (!UpdateDatabase)
            {
                var first = GetAll().OrderByDescending(e => e.ProductID).FirstOrDefault();
                var id = (first != null) ? first.ProductID : 0;

                product.ProductID = id + 1;

                if (product.CategoryID == null)
                {
                    product.CategoryID = 1;
                }

                if (product.Category == null)
                {
                    product.Category = new CategoryViewModel() { CategoryID = 1, CategoryName = "Beverages" };
                }

                GetAll().Insert(0, product);
            }
            else
            {
                var entity = new Product();

                entity.ProductName = product.ProductName;
                entity.UnitPrice = product.UnitPrice;
                entity.UnitsInStock = (short)product.UnitsInStock;
                entity.Discontinued = product.Discontinued;
                entity.CategoryID = product.CategoryID;

                if (entity.CategoryID == null)
                {
                    entity.CategoryID = 1;
                }

                if (product.Category != null)
                {
                    entity.CategoryID = product.Category.CategoryID;
                }

                entities.Products.Add(entity);
                entities.SaveChanges();

                product.ProductID = entity.ProductID;
            }
        }

        public void Update(ProductViewModel product)
        {
            if (!UpdateDatabase)
            {
                var target = One(e => e.ProductID == product.ProductID);

                if (target != null)
                {
                    target.ProductName = product.ProductName;
                    target.UnitPrice = product.UnitPrice;
                    target.UnitsInStock = product.UnitsInStock;
                    target.Discontinued = product.Discontinued;

                    if (product.CategoryID == null)
                    {
                        product.CategoryID = 1;
                    }

                    if (product.Category != null)
                    {
                        product.CategoryID = product.Category.CategoryID;
                    }
                    else
                    {
                        product.Category = new CategoryViewModel()
                        {
                            CategoryID = (int)product.CategoryID,
                            CategoryName = entities.Categories.Where(s => s.CategoryID == product.CategoryID).Select(s => s.CategoryName).First()
                        };
                    }
                    
                    target.CategoryID = product.CategoryID;
                    target.Category = product.Category;
                }
            }
            else
            {
                var entity = new Product();

                entity.ProductID = product.ProductID;
                entity.ProductName = product.ProductName;
                entity.UnitPrice = product.UnitPrice;
                entity.UnitsInStock = (short)product.UnitsInStock;
                entity.Discontinued = product.Discontinued;
                entity.CategoryID = product.CategoryID;

                if (product.Category != null)
                {
                    entity.CategoryID = product.Category.CategoryID;
                }

                entities.Products.Attach(entity);
                entities.Entry(entity).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        public void Destroy(ProductViewModel product)
        {
            if (!UpdateDatabase)
            {
                var target = GetAll().FirstOrDefault(p => p.ProductID == product.ProductID);
                if (target != null)
                {
                    GetAll().Remove(target);
                }
            }
            else
            {
                var entity = new Product();

                entity.ProductID = product.ProductID;

                entities.Products.Attach(entity);

                entities.Products.Remove(entity);

                var orderDetails = entities.Order_Details.Where(pd => pd.ProductID == entity.ProductID);

                foreach (var orderDetail in orderDetails)
                {
                    entities.Order_Details.Remove(orderDetail);
                }

                entities.SaveChanges();
            }
        }

        public ProductViewModel One(Func<ProductViewModel, bool> predicate)
        {
            return GetAll().FirstOrDefault(predicate);
        }

        public void Dispose()
        {
            entities.Dispose();
        }
    }
    
    
    
    
    
    
    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////
    /// </summary>

    public class GridController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();
        private readonly ProductService productService;

        public GridController()
        {
            productService = new ProductService(new DetergentsEntities());
        }
        public ActionResult Orders_Read([DataSourceRequest]DataSourceRequest request)
        {
            var result = db.Products;
            var result = Enumerable.Range(0, 50).Select(i => new Product()
            {
                ProductID = 
                Freight = i * 10,
                OrderDate = DateTime.Now.AddDays(i),
                ShipName = "ShipName " + i,
                ShipCity = "ShipCity " + i
            });

            return Json(result.ToDataSourceResult(request));
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
        public ActionResult Products_Create()
        {
            return Grid();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Create(Product product)
        
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Grid");
            }

            ViewBag.Category = new SelectList(db.Categories, "CategoryID", "CategoryName", product.Category);
            return View("Grid", productService.Read());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Update(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Grid");
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