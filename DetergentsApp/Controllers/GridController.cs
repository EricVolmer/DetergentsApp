using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DetergentsApp.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace DetergentsApp.Controllers
{
    public class GridController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();


        public ActionResult Grid()
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

            return View();
        }

        public ActionResult Products_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = db.Products;

                var vesselsList = result.Select(entity => new ProductViewModel
                    {
                    //    sheetTypeName = entity.SheetType.sheetTypeName,
                        productID = entity.productID,
                        productName = entity.productName,
                        productDescription = entity.productDescription,
                        EAN = entity.EAN,
                        categoryID = entity.Category.categoryID
                    })
                    .ToList();


                return Json(vesselsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
            try
            {
                var result = db.Products;

                var vesselsList = result.Select(entity => new ProductViewModel
                    {
                        productID = entity.productID,
                        productName = entity.productName,
                        productDescription = entity.productDescription,
                        EAN = entity.EAN,
                        categoryName = entity.Category.categoryName
                    })
                    .ToList();


                return Json(vesselsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Products_Destroy(Product product)
        {
            RouteValueDictionary routeValues;

            routeValues = this.GridRouteValues();

            return RedirectToAction("Grid", routeValues);
        }


        

        

        
        public ActionResult ToolbarTemplate_Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var result = db.Categories;
                var vesselsList = result.Select(entity => new ProductViewModel()
                    {
                        categoryID = entity.categoryID,
                        categoryName = entity.categoryName,
                    })
                    .ToList();
                
                return Json(vesselsList.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public ActionResult ToolbarTemplate_Categories()
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

            return Json(containerList, JsonRequestBehavior.AllowGet);
        }
    }
}