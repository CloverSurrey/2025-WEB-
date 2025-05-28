using System;
using System.Web.Mvc;
using Music_Shopping.Models;
using System.Net;
using System.Diagnostics;

namespace Music_Shopping.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ProductFactory _productFactory;

        public OrderController()
        {
            var context = new Music_ShoppingEntities();
            _orderService = new OrderService(context);
            _productFactory = new ProductFactory(context);
        }

        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as ShoppingCart;
            if (cart == null || cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            ViewBag.UserId = Session["UserId"];
            return View();
        }

        [HttpPost]
        public ActionResult CreateOrder(int userId, string address)
        {
            var cart = Session["Cart"] as ShoppingCart;
            if (cart == null || cart.Items.Count == 0)
            {
                return Json(new { success = false, message = "购物车为空" });
            }
            try
            {
                var order = _orderService.CreateOrder(cart, userId, address);
                if (order != null)
                {
                    // 清空购物车
                    cart.Clear();
                    Session["Cart"] = cart;

                    return Json(new { 
                        success = true, 
                        redirectUrl = Url.Action("OrderSuccess", "Order", new { orderId = order.order_id })
                    });
                }
                return Json(new { success = false, message = "创建订单失败" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "创建订单时发生错误：" + ex.Message });
            }
        }

        // GET: Order/OrderSuccess
        public ActionResult OrderSuccess(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return RedirectToAction("Index", "Cart");
            }

            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return HttpNotFound();
            }

            var orderDetails = _orderService.GetOrderDetails(orderId);
            ViewBag.OrderDetails = orderDetails;

            return View(order);
        }

        // GET: Order/MyOrders
        public ActionResult MyOrders()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "User");
            }

            int userId = int.Parse(Session["UserId"].ToString());
            Debug.WriteLine($"当前用户ID: {userId}");
            if (userId == 0)
            {
                return RedirectToAction("Login", "User");
            }

            var orders = _orderService.GetUserOrders(userId);
            return View(orders);
        }

        // GET: Order/OrderDetail/5
        public ActionResult OrderDetail(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return HttpNotFound();
            }

            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return HttpNotFound();
            }

            // 检查是否是当前用户的订单
            int userId = int.Parse(Session["UserId"].ToString());
            if (order.user_id != userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            var orderDetails = _orderService.GetOrderDetails(orderId);
            ViewBag.OrderDetails = orderDetails;

            return View(order);
        }
    }
} 