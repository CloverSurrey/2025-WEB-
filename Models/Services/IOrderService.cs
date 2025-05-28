using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music_Shopping.Models.Services
{
    internal interface IOrderService
    {
        order CreateOrder(ShoppingCart cart, int userId, string address);
        order GetOrderById(string orderId);
        List<OrderDetailViewModel> GetOrderDetails(string orderId);
        List<order> GetUserOrders(int userId);
    }
}
