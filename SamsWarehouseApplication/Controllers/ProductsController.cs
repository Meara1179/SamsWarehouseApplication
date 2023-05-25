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

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsController/Create
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

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
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

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductsController/Delete/5
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
