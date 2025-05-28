 using System.Collections.Generic;

namespace Music_Shopping.Models.Services // 更新命名空间
{
    public interface IProductService
    {
        IEnumerable<IProduct> GetAllProducts();
        IProduct GetProductById(string productId);
        IEnumerable<IProduct> GetFilteredProducts(string language, string musicGenre);
        List<string> GetMusicGenres();
        List<string> GetLanguages();
    }
}