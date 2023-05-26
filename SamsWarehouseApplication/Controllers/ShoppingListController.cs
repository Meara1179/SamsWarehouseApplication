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
                Text = x.ShoppingListName + " - Created on: " + x.ShoppingListDate,
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



        public async Task<IActionResult> GetShoppingListItems([FromQuery] int listID)
        {
            List<Product> shoppingListProducts = _shoppingContext.ShoppingItems.Include(x => x.Product).ThenInclude(x => x.ProductList).Where(x => x.ShoppingListId == listID).Select(x => x.Product).ToList();

            return PartialView("_ShoppingItemsList", shoppingListProducts);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewShoppingList([FromBody] string listName)
        {
            int? id = HttpContext.Session.GetInt32("AppUserId");
            if (!id.HasValue)
            {
                return Unauthorized();
            }

            if (_shoppingContext.ShoppingLists.Any(x => x.ShoppingListName == listName && x.AppUserId == id))
            {
                return BadRequest();
            }

            ShoppingList newList = new ShoppingList()
            {
                ShoppingListName = listName,
                AppUserId = id.Value,
                ShoppingListDate = DateTime.Now,
            };
            _shoppingContext.Add(newList);
            _shoppingContext.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveShoppingList([FromQuery] int listID)
        {
            var shoppingList = _shoppingContext.ShoppingLists.Where(x => x.ShoppingListId == listID).FirstOrDefault();

            if (shoppingList != null)
            {
                _shoppingContext.Remove(shoppingList);
                _shoppingContext.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddShoppingListItem([FromBody] ShoppingListItem item)
        {
            if (_shoppingContext.ShoppingItems.Any(x => x.ShoppingListId == item.ShoppingListId && x.ProductId == item.ProductId)) 
            {
                return BadRequest();
            }

            _shoppingContext.ShoppingItems.Add(item);
            _shoppingContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveShoppingListItem([FromBody] ShoppingListItem item)
        {
            var shoppingListItems = _shoppingContext.ShoppingItems.Where(x => x.ShoppingListId == item.ShoppingListId &&
            x.ProductId == item.ProductId).FirstOrDefault();

            if (shoppingListItems != null)
            {
                _shoppingContext.Remove(shoppingListItems);
                _shoppingContext.SaveChanges();
                return Ok();
            }

            return BadRequest();
        }
    }
}
