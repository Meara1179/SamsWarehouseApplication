using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SamsWarehouseApplication.Models;

namespace SamsWarehouseApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShoppingContext _shoppingContext;
        public ProductsController(ShoppingContext shoppingContext)
        {
            _shoppingContext = shoppingContext;
        }

        // GET: ProductsController
        public ActionResult Index()
        {
            return View(_shoppingContext.Products.AsEnumerable());
        }

        [HttpGet]
        public async Task<ActionResult> ProductsTablePartial()
        {
            var productsData = _shoppingContext.Products.AsEnumerable();

            return PartialView("_ProductsTable", productsData);
        }
    }
}
