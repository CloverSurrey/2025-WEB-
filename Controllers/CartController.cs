using System;
using System.Web.Mvc;
using Music_Shopping.Models;

namespace Music_Shopping.Controllers
{
    public class CartController : Controller
    {
        private readonly ProductFactory _productFactory;

        public CartController()
        {
            var context = new Music_ShoppingEntities();
            _productFactory = new ProductFactory(context);
        }

        public ActionResult Index()
        {
            var cart = Session["Cart"] as ShoppingCart ?? new ShoppingCart();
            return View(cart);
        }

        [HttpPost]
        public ActionResult AddToCart(string productId, int quantity)
        {
            var product = _productFactory.CreateProduct(productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            var cart = Session["Cart"] as ShoppingCart ?? new ShoppingCart();
            cart.AddItem(product, quantity);
            Session["Cart"] = cart;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateQuantity(string productId, int quantity)
        {
            var cart = Session["Cart"] as ShoppingCart;
            if (cart != null)
            {
                cart.UpdateQuantity(productId, quantity);
                Session["Cart"] = cart;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveItem(string productId)
        {
            var cart = Session["Cart"] as ShoppingCart;
            if (cart != null)
            {
                cart.RemoveItem(productId);
                Session["Cart"] = cart;
            }

            return RedirectToAction("Index");
        }

        public ActionResult Clear()
        {
            var cart = Session["Cart"] as ShoppingCart;
            if (cart != null)
            {
                cart.Clear();
                Session["Cart"] = cart;
            }

            return RedirectToAction("Index");
        }
    }
}