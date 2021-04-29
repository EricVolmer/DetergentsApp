using System;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.UI;

namespace DetergentsApp.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CategoryCreate([DataSourceRequest] DataSourceRequest request,
            ProductViewModel category)
        {
            var newCategory = new Category()
            {
                categoryName = category.categoryName
            };
            try
            {
                db.Categories.Add(newCategory);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return View("Index");
        }
    }
}