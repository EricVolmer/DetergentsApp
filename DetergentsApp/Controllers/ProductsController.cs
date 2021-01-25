﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace DetergentsApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "categoryID", "categoryName");
            ViewBag.SheetTypeID = new SelectList(db.SheetTypes, "sheetTypeID", "sheetTypeName");

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "EAN,productName,productDescription,categoryID")]
            Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category = new SelectList(db.Categories, "categoryID", "categoryName", product.Category);
            //    ViewBag.Category = new SelectList(db.SheetTypes, "sheetTypeID", "sheetTypeName", product.SheetType);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            ViewBag.categoryID = new SelectList(db.Categories, "categoryID", "categoryName", product.Category);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "EAN,title,productName,productDescription,validFrom")]
            Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "categoryID", "categoryName", product.Category);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }

        ///////////////////////////////////////////////////////////////////////
        /// 
        public ActionResult Products_Read([DataSourceRequest] DataSourceRequest request)
        {
            return Json(GetProducts().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get_Product(int ID)
        {
            return Json(GetProducts().Where(product => product.productID == ID).SingleOrDefault(),
                JsonRequestBehavior.AllowGet);
        }


        private static IQueryable<ProductViewModel> GetProducts()
        {
            var northwind = new DetergentsEntities();
            var products = northwind.Products.Select(product => new ProductViewModel
            {
                productID = product.productID,
                productName = product.productName,
                categoryName = product.Category.categoryName
            });

            return products;
        }
    }
}