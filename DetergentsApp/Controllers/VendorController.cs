using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DetergentsApp.Models;

namespace DetergentsApp.Controllers
{
    public class VendorController : Controller
    {
        private DetergentsEntities db = new DetergentsEntities();

        // GET: Vendor
        public ActionResult Index()
        {
            return View(db.VendorSet.ToList());
        }

    }
}
