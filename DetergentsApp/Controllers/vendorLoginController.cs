using System.Linq;
using System.Web.Mvc;
using DetergentsApp.Models;


namespace DetergentsApp.Controllers
{
    public class vendorLoginController : Controller
    {
        public ActionResult vendorLogin()  
        {  
            return View();  
        }  
  
        [HttpPost]  
        [ValidateAntiForgeryToken]  
        public ActionResult vendorLogin(vendorLogin objUser)   
        {  
            if (ModelState.IsValid)   
            {  
                using(DetergentsEntities db = new DetergentsEntities())  
                {  
                    var obj = db.vendorLogin.FirstOrDefault(a => a.userName.Equals(objUser.userName) && a.password.Equals(objUser.password));  
                    if (obj != null)  
                    {  
                        Session["UserName"] = obj.userName.ToString();  
                       // return RedirectToAction("UserDashBoard");  
                    }  
                }  
            }  
            return View(objUser);  
        }  
  
        // public ActionResult UserDashBoard()  
        // {  
        //     if (Session["UserID"] != null)  
        //     {  
        //         return View();  
        //     } else  
        //     {  
        //         return RedirectToAction("Login");  
        //     }  
        // }
    }
}