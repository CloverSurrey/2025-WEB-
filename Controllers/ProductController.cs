using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Music_Shopping.Models;

namespace Music_Shopping.Controllers
{
    public class ProductController : Controller
    {
        private readonly Music_ShoppingEntities _db;
        private readonly ProductFactory _productFactory;

        public ProductController()
        {
            _db = new Music_ShoppingEntities();
            _productFactory = new ProductFactory(_db);
        }

        public ActionResult Index(string language = null, string MusicGenre = null)
        {
            try
            {
                // 获取所有产品ID
                var productIds = _db.products.Select(p => p.product_id).ToList();
                Debug.WriteLine($"产品ID列表: {string.Join(", ", productIds)}");
                
                // 使用ProductFactory创建IProduct对象列表
                var products = productIds
                    .Select(id => _productFactory.CreateProduct(id))
                    .Where(p => p != null)
                    .ToList();
                
                Debug.WriteLine($"创建的产品数量: {products.Count}");
                foreach (var product in products)
                {
                    Debug.WriteLine($"产品: ID={product.ProductId}, 名称={product.ProductName}, 类型={product.Type}");
                }

                // 获取所有唱片记录
                var records = _db.products_records
                    .Where(r => productIds.Contains(r.product_id))
                    .ToList();
                
                Debug.WriteLine($"唱片记录数量: {records.Count}");

                // 根据语言筛选
                if (!string.IsNullOrEmpty(language))
                {
                    var langIds = records
                        .Where(r => r.language == language)
                        .Select(r => r.product_id)
                        .ToList();
                    products = products.Where(p => langIds.Contains(p.ProductId)).ToList();
                    Debug.WriteLine($"语言筛选后产品数量: {products.Count}");
                }

                // 根据类型筛选
                if (!string.IsNullOrEmpty(MusicGenre))
                {
                    var genreIds = records
                        .Where(r => r.music_genre == MusicGenre)
                        .Select(r => r.product_id)
                        .ToList();
                    products = products.Where(p => genreIds.Contains(p.ProductId)).ToList();
                    Debug.WriteLine($"类型筛选后产品数量: {products.Count}");
                }

                // 设置ViewBag数据
                ViewBag.MusicGenres = records
                    .Select(r => r.music_genre)
                    .Distinct()
                    .OrderBy(g => g)
                    .ToList();
                ViewBag.Languages = records
                    .Select(r => r.language)
                    .Distinct()
                    .OrderBy(l => l)
                    .ToList();

                // 保存当前选中的筛选条件
                ViewBag.SelectedMusicGenre = MusicGenre;
                ViewBag.SelectedLanguage = language;

                return View(products);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"发生错误: {ex.Message}");
                Debug.WriteLine($"错误详情: {ex.StackTrace}");
                return View(new List<IProduct>());
            }
        }

        public ActionResult Details(string id)
        {
            var product = _productFactory.CreateProduct(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}