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
        
        /// <summary>
        /// Returns the Login/Index.cshtml View
        /// </summary>
        /// <returns>View</returns>
        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Checks if the inputted values match that of a user stored in the database, if it does the method redirects tthe user back to the Home page, 
        /// otherwise it returns the user to the login page.
        /// If the user is successfully authenticated the User's email address and ID will be stored in the session, as well as an Authenticated flag. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>RedirectToAction</returns>
        [HttpPost]
        public IActionResult Index(AppUserDTO user)
        {
            var users = _shoppingContext.AppUsers.Where(x => x.UserEmail == user.UserEmail && x.UserPasswordHash == user.UserPassword).FirstOrDefault();

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

        /// <summary>
        /// Clears the current session of all data, logging the user out, then redirects back to Home.
        /// </summary>
        /// <returns>RedirectToAction</returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
