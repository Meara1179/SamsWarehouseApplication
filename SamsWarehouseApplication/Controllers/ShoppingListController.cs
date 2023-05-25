using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SamsWarehouseApplication.Models;

namespace SamsWarehouseApplication.Controllers
{
    public class ShoppingListController : Controller
    {
        private readonly ShoppingContext _shoppingContext;
        public ShoppingListController(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        // GET: ShoppingListController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetInt32("AppUserId") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public async Task<ActionResult> ShoppingListDropDownList()
        {
            var user = _shoppingContext.AppUsers.Where(x => x.AppUserId == HttpContext.Session.GetInt32("AppUserId")).Include(x => x.UserShoppingList).FirstOrDefault();
            if (user == null)
            {
                return BadRequest();
            }

            var selectList = user.UserShoppingList.Select(x => new SelectListItem
            {
                Text = x.ShoppingListName,
                Value = x.ShoppingListId.ToString()
            });

            ViewBag.SelectList = selectList;

            return PartialView("_ShoppingListDropDownList");
        }

        [HttpGet]
        public async Task<ActionResult> ShoppingItemsList()
        {
            if (HttpContext.Session.GetInt32("AppUserId") != null)
            {
                var shoppingListData = _shoppingContext.ShoppingLists.Where(x => x.AppUserId == HttpContext.Session.GetInt32("AppUserId")).AsEnumerable();

                return PartialView("_ShoppingListPartial");
            }
            else
            {
                return Unauthorized();
            }
        }

        // GET: ShoppingListController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShoppingListController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShoppingListController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShoppingListController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShoppingListController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShoppingListController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShoppingListController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
