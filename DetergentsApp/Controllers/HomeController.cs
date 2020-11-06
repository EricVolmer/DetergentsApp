using System.Linq;
using System.Web.Mvc;
using DetergentsApp.Models;

namespace DetergentsApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Cascading_Get_Categories()
        {
            var northwind = new DetergentsEntities();

            return Json(
                northwind.Categories.Select(c => new {CategoryId = c.categoryID, CategoryName = c.categoryName}),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult Cascading_Get_Products(int? categories)
        {
            var northwind = new DetergentsEntities();
            var products = northwind.Products.AsQueryable();

            if (categories != null) products = products.Where(p => p.categoryID == categories);

            return Json(products.Select(p => new {ProductID = p.productID, ProductName = p.productName}),
                JsonRequestBehavior.AllowGet);
        }
    }
}