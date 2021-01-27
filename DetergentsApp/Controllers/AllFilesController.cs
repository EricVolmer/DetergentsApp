using System.Web.Mvc;
using DetergentsApp.Models;

namespace DetergentsApp.Controllers
{
    public class AllFilesController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();


        public ActionResult AllFiles()
        {
            return View();
        }
    }
}