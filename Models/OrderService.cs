using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Diagnostics;

namespace Music_Shopping.Models
{
    public class OrderService
    {
        private readonly Music_ShoppingEntities _context;

        public OrderService(Music_ShoppingEntities context)
        {
            _context = context;
        }

        private string GenerateOrderId()
        {
            return DateTime.Now.ToString("yyyyMMddHHmm") + new Random().Next(10, 99).ToString();
        }

        public order CreateOrder(ShoppingCart cart, int userId, string address)
        {
            var orderId = GenerateOrderId();
            var order = new order
            {
                order_id = orderId,
                user_id = userId,
                address = address,
                create_time = DateTime.Now,
                status = "待付款",
                total_price = cart.GetTotal()
            };

            // 创建订单详情
            var orderDetails = cart.Items.Select(item => new order_details
            {
                order_id = order.order_id,
                product_id = item.ProductId,
                price = item.Price,
                amount = item.Quantity
            }).ToList();

            _context.orders.Add(order);
            _context.order_details.AddRange(orderDetails);
            _context.SaveChanges();

            return order;
        }

        public order GetOrderById(string orderId)
        {
            return _context.orders.FirstOrDefault(o => o.order_id.Equals(orderId));
        }

        public List<OrderDetailViewModel> GetOrderDetails(string orderId)
        {
            using (var db = new Music_ShoppingEntities())
            {
                var details = (from od in db.order_details
                             join p in db.products on od.product_id equals p.product_id
                             where od.order_id == orderId
                             select new OrderDetailViewModel
                             {
                                 ProductId = od.product_id,
                                 ProductName = p.product_name,
                                 Price = od.price ?? 0,
                                 Amount = od.amount ?? 0
                             }).ToList();
                return details;
            }
        }

        public List<order> GetUserOrders(int userId)
        {
            return _context.orders.Where(o => o.user_id == userId).OrderByDescending(o => o.create_time).ToList();
        }
    }
} 