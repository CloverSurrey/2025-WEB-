using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Music_Shopping.Models;

namespace Music_Shopping.Models.Services
{
    public class ProductService : IProductService
    {
        private readonly Music_ShoppingEntities _db;
        private readonly ProductFactory _productFactory;

        public ProductService(Music_ShoppingEntities dbContext)
        {
            _db = dbContext;
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
            var allProducts = GetAllProducts().ToList(); // 获取所有产品

            var recordProducts = allProducts.OfType<RecordProduct>().ToList();

            // 根据语言筛选
            if (!string.IsNullOrEmpty(language))
            {
                recordProducts = recordProducts.Where(r => r.Language == language).ToList();
            }

            // 根据音乐流派筛选
            if (!string.IsNullOrEmpty(musicGenre))
            {
                recordProducts = recordProducts.Where(r => r.MusicGenre == musicGenre).ToList();
            }

            // 如果进行了筛选，且筛选的是 RecordProduct 特有的属性，则返回筛选后的 RecordProduct
            // 否则，如果 language 和 musicGenre 都为空，说明不需要筛选特定属性，返回所有产品
            if (!string.IsNullOrEmpty(language) || !string.IsNullOrEmpty(musicGenre))
            {
                return recordProducts;
            }
            return allProducts; 
        }

        public List<string> GetMusicGenres()
        {
            // 确保只从 RecordProduct 中获取音乐流派
            var productIds = _db.products
                                .Where(p => p.type.Contains("RCD")) // 假设RCD表示唱片类型
                                .Select(p => p.product_id)
                                .ToList();

            return _db.products_records
                .Where(r => productIds.Contains(r.product_id) && r.music_genre != null)
                .Select(r => r.music_genre)
                .Distinct()
                .OrderBy(g => g)
                .ToList();
        }

        public List<string> GetLanguages()
        {
             // 确保只从 RecordProduct 中获取语言
            var productIds = _db.products
                                .Where(p => p.type.Contains("RCD")) // RCD表示唱片类型
                                .Select(p => p.product_id)
                                .ToList();
            return _db.products_records
                .Where(r => productIds.Contains(r.product_id) && r.language != null)
                .Select(r => r.language)
                .Distinct()
                .OrderBy(l => l)
                .ToList();
        }
    }
}