using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SamsWarehouseApplication.Models;

namespace SamsWarehouseApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly ShoppingContext _shoppingContext;
        public LoginController(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

                [HttpPost]
        public IActionResult Index(AppUserDTO user)
        {
            var users = _shoppingContext.AppUsers.Where(x => x.UserEmail == user.UserEmail && x.UserPassword == user.UserPassword).FirstOrDefault();

            if (users == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                HttpContext.Session.SetInt32("AppUserId", users.AppUserId);
                HttpContext.Session.SetString("UserEmail", users.UserEmail.ToString());
                HttpContext.Session.SetString("Authenticated", "True");
            }
            return RedirectToAction("Index", "Home");
        }
        
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
