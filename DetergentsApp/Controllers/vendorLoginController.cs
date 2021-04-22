using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using DetergentsApp.Models;

namespace DetergentsApp.Controllers
{
    public class vendorLoginController : Controller
    {
        private readonly DetergentsEntities db = new DetergentsEntities();

        // [Authorize(Roles = "Admin")]
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

        // public ActionResult vendorLogin()
        // {
        //     return View();
        // }
        //
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public ActionResult vendorLogin(string userName, string password)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var f_password = GetMD5(password);
        //         var data = db.vendorLogin.Where(s => s.userName.Equals(userName) && s.password.Equals(f_password))
        //             .ToList();
        //         if (data.Count() > 0)
        //         {
        //             //add session
        //             FormsAuthentication.SetAuthCookie(.userName, false); // set the formauthentication cookie  
        //
        //             Session["UserName"] = data.FirstOrDefault().userName;
        //             Session["Id"] = data.FirstOrDefault().Id;
        //             return RedirectToAction("Index");
        //         }
        //
        //         ViewBag.error = "Login failed";
        //          return RedirectToAction("vendorLogin");
        //     }
        //
        //     return View();
        // }

        public ActionResult vendorLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult vendorLogin(vendorLogin _login, string userName, string password)
        {
            if (ModelState.IsValid) //validating the user inputs  
            {
                var f_password = GetMD5(password);
                var isExist = false;
                using (var _entity = new DetergentsEntities()) // out Entity name is "SampleMenuMasterDBEntites"  
                {
                    isExist = _entity.vendorLogin
                        .Any(x => x.userName.Trim().ToLower() == _login.userName.Trim().ToLower() &&
                                  x.password ==
                                  f_password); //validating the user name in tblLogin table whether the user name is exist or not  
                    if (isExist)
                    {
                        var _loginCredentials = _entity.vendorLogin.ToList()
                            .Where(x => x.userName.Trim().ToLower() == _login.userName.Trim().ToLower()).Select(x =>
                                new vendorLogin
                                {
                                    userName = x.userName,
                                    Id = x.Id,
                                    password = x.password
                                }).FirstOrDefault(); // Get the login user details and bind it to LoginModels class  
                        //Get the Menu details from entity and bind it in MenuModels list.  
                        FormsAuthentication.SetAuthCookie(_loginCredentials.userName,
                            false); // set the formauthentication cookie  
                        Session["LoginCredentials"] =
                            _loginCredentials; // Bind the _logincredentials details to "LoginCredentials" session  
                        Session["UserName"] = _loginCredentials.userName;
                        return RedirectToAction("Index", "Vendor");
                    }

                    ViewBag.ErrorMsg = "Please enter the valid credentials!...";
                    return View();
                }
            }

            return View();
        }

//Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.RemoveAll();
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