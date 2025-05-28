 using System;
using System.Web;
// HttpSessionStateBase 在 System.Web 中，但通常我们希望服务层不直接依赖HttpContext
// 更好的做法是传递一个包装器或者直接操作 Session 的方法

namespace Music_Shopping.Models.Services // 更新命名空间
{
    public class CartService : ICartService
    {
        private readonly IProductService _productService;
        // 注意：直接在服务层使用 HttpContext.Current.Session 是不太推荐的做法，
        // 它使得服务层与Web层紧密耦合，难以测试。
        // 更优的方案是通过构造函数注入一个ISessionWrapper之类的抽象，或者让Controller负责Session的读写。
        // 但为了快速实现，这里暂时保留直接访问的方式，之后可以再优化。
        private HttpSessionStateBase GetSession()
        {
            // 如果在非Web请求上下文中使用此服务（例如单元测试），HttpContext.Current 可能为 null
            if (HttpContext.Current == null)
            {
                // 可以考虑返回一个模拟的Session对象或抛出异常，具体取决于应用需求
                throw new InvalidOperationException("HttpContext is not available in the current context.");
            }
            return new HttpSessionStateWrapper(HttpContext.Current.Session);
        }

        public CartService(IProductService productService)
        {
            _productService = productService;
        }

        public ShoppingCart GetCart()
        {
            var session = GetSession();
            return session["Cart"] as ShoppingCart ?? new ShoppingCart();
        }

        public void AddToCart(string productId, int quantity)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
            {
                // 通常服务层应该抛出更具体的业务异常
                throw new ArgumentException("Product not found", nameof(productId));
            }

            var cart = GetCart();
            cart.AddItem(product, quantity);
            SaveCart(cart);
        }

        public void RemoveFromCart(string productId)
        {
            var cart = GetCart();
            cart.RemoveItem(productId);
            SaveCart(cart);
        }

        public void UpdateQuantity(string productId, int quantity)
        {
            var cart = GetCart();
            if (quantity <= 0) // 确保数量有效
            {
                cart.RemoveItem(productId); // 如果数量小于等于0，则移除商品
            }
            else
            {
                cart.UpdateQuantity(productId, quantity);
            }
            SaveCart(cart);
        }

        public void ClearCart()
        {
            var cart = GetCart();
            cart.Clear();
            SaveCart(cart);
        }

        private void SaveCart(ShoppingCart cart)
        {
            var session = GetSession();
            session["Cart"] = cart;
        }
    }
}