using System;
using System.Web.Mvc;
using Music_Shopping.Models;
using Music_Shopping.Models.Services;

namespace Music_Shopping.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductService _productService;

        public CartController()
        {
            var context = new Music_ShoppingEntities();
            _productService = new ProductService(context);
        }

        private ShoppingCart GetOrCreateCart()
        {
            return Session["Cart"] as ShoppingCart ?? new ShoppingCart();
        }

        private void SaveCart(ShoppingCart cart)
        {
            Session["Cart"] = cart;
        }

        public ActionResult Index()
        {
            var cart = GetOrCreateCart();
            return View(cart);
        }

        [HttpPost]
        public ActionResult AddToCart(string productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            var cart = GetOrCreateCart();
            cart.AddItem(product, quantity);
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateQuantity(string productId, int quantity)
        {
            if (quantity <= 0)
            {
                return RemoveItem(productId);
            }

            var cart = GetOrCreateCart();
            cart.UpdateQuantity(productId, quantity);
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveItem(string productId)
        {
            var cart = GetOrCreateCart();
            cart.RemoveItem(productId);
            SaveCart(cart);

            return RedirectToAction("Index");
        }

        public ActionResult Clear()
        {
            var cart = GetOrCreateCart();
            cart.Clear();
            SaveCart(cart);

            return RedirectToAction("Index");
        }
    }
}