using System;
using System.Collections.Generic;
using System.Linq;
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
            return View();
        }
    }
}