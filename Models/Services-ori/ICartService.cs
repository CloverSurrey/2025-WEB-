 using System;

namespace Music_Shopping.Models.Services // 更新命名空间
{
    public interface ICartService
    {
        ShoppingCart GetCart();
        void AddToCart(string productId, int quantity);
        void RemoveFromCart(string productId);
        void UpdateQuantity(string productId, int quantity);
        void ClearCart();
        // SaveCart 可能不是公共接口的一部分，因为通常由服务内部管理状态
        // void SaveCart(ShoppingCart cart); 
    }
}