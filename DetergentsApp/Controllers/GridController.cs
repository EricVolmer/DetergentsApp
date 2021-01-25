using System.Web.Mvc;
using DetergentsApp.Models;

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