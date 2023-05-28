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

        /// <summary>
        /// Checks to see a user ID is currently stored in the session, if not the User is redirected to the login page, otherwise the method returns 
        /// the ShoppingList/Index.cshtml View.
        /// </summary>
        /// <returns>View/RedirectToAction</returns>
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

        /// <summary>
        /// Grabs the User object with the matching ID thats stored in the session and includes all Shopping Lists under that user throught the Navigation varible.
        /// If no user is found, returns a bad request otherwise creates a list of of SelectListItem that contain the Shopping Lists name and creation date as the text,
        /// and the Shopping List ID as the value. Then adds the select list to the viewbag and adds the ShoppingList/_ShoppingListDropDownList Partial View 
        /// to the Index View.
        /// </summary>
        /// <returns>PartialView/BadRequest</returns>
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

        /// <summary>
        /// Checks if a User ID is stored in session, if so uses the ID to query all shopping lists under that User and pass it through into a Partial View of
        /// ShoppingList/_ShoppingListPartial.cshtml.
        /// If a User ID is not stored in session, returns a 401 Unauthorized status code.
        /// </summary>
        /// <returns>PartialView/Unauthorized</returns>
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

        /// <summary>
        /// Queries a list of products from a shopping list using the Shopping List Items connecting table and passes it through to 
        /// the ShoppingList/"_ShoppingItemsList.cshtml Partial View
        /// </summary>
        /// <param name="listID"></param>
        /// <returns>PartialView</returns>
        public async Task<IActionResult> GetShoppingListItems([FromQuery] int listID)
        {
            List<Product> shoppingListProducts = _shoppingContext.ShoppingItems.Include(x => x.Product).ThenInclude(x => x.ProductList).Where(x => x.ShoppingListId == listID).Select(x => x.Product).ToList();

            return PartialView("_ShoppingItemsList", shoppingListProducts);
        }

        /// <summary>
        /// Verifies that a User ID is stored in the session before procedding, if not returns a 401 Unauthorized status code. If one is found in the session
        /// it checks if the list name already exists, if it does it returns a 409 status code. If there is no conflict in the name then a new shopping list is created
        /// using the supplied list name with the current data and time and the ID of the currently logged in user.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns>Unauthorized/BadRequest/Ok</returns>
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
                return StatusCode(409);
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

        /// <summary>
        /// Removes the shopping list that matches the supplied list id.
        /// </summary>
        /// <param name="listID"></param>
        /// <returns>Ok/BadRequest</returns>
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

        /// <summary>
        /// Checks if the same product has already been added to list and returns a 400 status code if a duplicate is found. If no duplicate is found then 
        /// then the item is posted to the database and an Ok result is returned.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Ok</returns>
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

        /// <summary>
        /// Finds a shopping list item that matches the same ID as the supplied item and if one is found, remove it from the database and return an Ok status.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Ok/BadRequest</returns>
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

        /// <summary>
        /// Finds a shopping list item that has the same ID as the supplied item and replaces the original's quantity with the new one, then Puts it back onto the database.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Ok/BadRequest</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateShoppingListItemQuantity([FromBody] ShoppingListItem item)
        {
            var shoppingListItems = _shoppingContext.ShoppingItems.Where(x => x.ShoppingListId == item.ShoppingListId &&
            x.ProductId == item.ProductId).FirstOrDefault();

            if (shoppingListItems != null)
            {
                shoppingListItems.Quantity = item.Quantity;

                _shoppingContext.Update(shoppingListItems);
                _shoppingContext.SaveChanges();

                return Ok();
            }

            return BadRequest();
        }
    }
}
