using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
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
            var newCategory = new Category
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

        public ActionResult CategoryRead([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = db.Categories;

                var list = result.Select(entity => new ProductViewModel
                    {
                        categoryID = entity.categoryID,
                        categoryName = entity.categoryName
                    })
                    .ToList();
                return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CategoryUpdate([DataSourceRequest] DataSourceRequest request, Category category)
        {
            if (ModelState.IsValid)
            {
                var entity = db.Categories.Find(category.categoryID);
                if (entity != null)
                {
                    entity.categoryID = category.categoryID;
                    entity.categoryName = category.categoryName;

                    try
                    {
                        db.Entry(entity).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new[] {category}.ToDataSourceResult(request, ModelState));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            return Json(new[] {category}.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CategoryDestroy([DataSourceRequest] DataSourceRequest request, Category category)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category
                {
                    categoryID = category.categoryID,
                    categoryName = category.categoryName
                };

                db.Categories.Attach(entity);
                db.Categories.Remove(entity);
                db.SaveChanges();
            }

            return Json(new[] {category}.ToDataSourceResult(request, ModelState));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}