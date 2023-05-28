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

        /// <summary>
        /// Returns the Products/Index.cshtml View while passing through the list of products as an IEnumerable.
        /// </summary>
        /// <returns>View</returns>
        // GET: ProductsController
        public ActionResult Index()
        {
            return View(_shoppingContext.Products.AsEnumerable());
        }

        /// <summary>
        /// Adds the Products/_ProductsTable.cshtml Partial View to the Index View while passing through the list of products as an IEnumerable.
        /// </summary>
        /// <returns>PartialView</returns>
        [HttpGet]
        public async Task<ActionResult> ProductsTablePartial()
        {
            var productsData = _shoppingContext.Products.AsEnumerable();

            return PartialView("_ProductsTable", productsData);
        }
    }
}
