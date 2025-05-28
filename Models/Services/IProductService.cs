using System.Collections.Generic;
using Music_Shopping.Models; // For IProduct

namespace Music_Shopping.Models
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