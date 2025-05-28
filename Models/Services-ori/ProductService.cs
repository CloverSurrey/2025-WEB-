 using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Music_Shopping.Models.Services // 更新命名空间
{
    public class ProductService : IProductService
    {
        private readonly Music_ShoppingEntities _db;
        private readonly ProductFactory _productFactory;

        public ProductService(Music_ShoppingEntities db)
        {
            _db = db;
            _productFactory = new ProductFactory(_db);
        }

        public IProduct GetProductById(string productId)
        {
            return _productFactory.CreateProduct(productId);
        }

        public IEnumerable<IProduct> GetAllProducts()
        {
            var productIds = _db.products.Select(p => p.product_id).ToList();
            var products = productIds
                .Select(id => _productFactory.CreateProduct(id))
                .Where(p => p != null)
                .ToList();
            return products;
        }

        public IEnumerable<IProduct> GetFilteredProducts(string language, string musicGenre)
        {
            var allProducts = GetAllProducts().ToList(); 

            var recordProductsQuery = allProducts.OfType<RecordProduct>();

            if (!string.IsNullOrEmpty(language))
            {
                recordProductsQuery = recordProductsQuery.Where(r => r.Language == language);
            }

            if (!string.IsNullOrEmpty(musicGenre))
            {
                recordProductsQuery = recordProductsQuery.Where(r => r.MusicGenre == musicGenre);
            }
            
            // 如果筛选条件不为空，则表示用户希望根据唱片特有属性筛选
            if (!string.IsNullOrEmpty(language) || !string.IsNullOrEmpty(musicGenre))
            {
                return recordProductsQuery.ToList<IProduct>();
            } 
            // 否则返回所有产品
            return allProducts;
        }

        public List<string> GetMusicGenres()
        {
            var productIds = _db.products
                                .Where(p => p.type != null && p.type.IndexOf("RCD", StringComparison.OrdinalIgnoreCase) >= 0)
                                .Select(p => p.product_id)
                                .ToList();

            return _db.products_records
                .Where(r => productIds.Contains(r.product_id) && !string.IsNullOrEmpty(r.music_genre))
                .Select(r => r.music_genre)
                .Distinct()
                .OrderBy(g => g)
                .ToList();
        }

        public List<string> GetLanguages()
        {
            var productIds = _db.products
                                .Where(p => p.type != null && p.type.IndexOf("RCD", StringComparison.OrdinalIgnoreCase) >= 0)
                                .Select(p => p.product_id)
                                .ToList();
            return _db.products_records
                .Where(r => productIds.Contains(r.product_id) && !string.IsNullOrEmpty(r.language))
                .Select(r => r.language)
                .Distinct()
                .OrderBy(l => l)
                .ToList();
        }
    }
}