using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using DetergentsApp.Models;

namespace DetergentsApp.Controllers
{
    public class vendorLoginController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();

        // GET
        public ActionResult Index()
        {
            if (Session["Id"] != null)
                return View();
            return RedirectToAction("vendorLogin");
        }

        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(vendorLogin _user)
        {
            if (ModelState.IsValid)
            {
                var check = db.vendorLogin.FirstOrDefault(s => s.userName == _user.userName);
                if (check == null)
                {
                    _user.password = GetMD5(_user.password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.vendorLogin.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.error = "Email already exists";
                return View();
            }

            return View();
        }

        public ActionResult vendorLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult vendorLogin(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var data = db.vendorLogin.Where(s => s.userName.Equals(userName) && s.password.Equals(f_password))
                    .ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["UserName"] = data.FirstOrDefault().userName;
                    Session["Id"] = data.FirstOrDefault().Id;
                    return RedirectToAction("Index");
                }

                ViewBag.error = "Login failed";
                // return RedirectToAction("vendorLogin");
            }

            return View();
        }

        //Logout
        public ActionResult Logout()
        {
            Session.Clear(); //remove session
            return RedirectToAction("vendorLogin");
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var fromData = Encoding.UTF8.GetBytes(str);
            var targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (var i = 0; i < targetData.Length; i++) byte2String += targetData[i].ToString("x2");
            return byte2String;
        }
    }
}