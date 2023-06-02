using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SamsWarehouseApplication.Models;
using SamsWarehouseApplication.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using System.Threading;


namespace SamsWarehouseApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly ShoppingContext _shoppingContext;
        private readonly AuthRepository _authRepository;

        public LoginController(ShoppingContext shoppingContext, AuthRepository authRepository)
        {
            _shoppingContext = shoppingContext;
            _authRepository = authRepository;
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
        public async Task<IActionResult> Index(AppUserDTO user)
        {
            var users = _authRepository.Authenticate(user);

            if (users == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, users.AppUserId.ToString()),
                new Claim(ClaimTypes.Name, users.UserEmail),
                new Claim(ClaimTypes.Role, users.AppUserRole)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
            };

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            Thread.CurrentPrincipal = claimsPrincipal;

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);


            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Clears the current session of all data, logging the user out, then redirects back to Home.
        /// </summary>
        /// <returns>RedirectToAction</returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
