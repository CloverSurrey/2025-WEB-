using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Music_Shopping.Models;
using Music_Shopping.Models.Services;

namespace Music_Shopping.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        // 无参数构造函数（当没有DI容器时使用）
        public ProductController()
        {
            var dbContext = new Music_ShoppingEntities();
            _productService = new ProductService(dbContext);
        }

        // 用于依赖注入的构造函数
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public ActionResult Index(string language = null, string musicGenre = null)
        {
            // 使用服务层获取筛选后的产品列表
            var products = _productService.GetFilteredProducts(language, musicGenre); 
        
            // 使用服务层方法获取筛选条件数据
            ViewBag.MusicGenres = _productService.GetMusicGenres();
            ViewBag.Languages = _productService.GetLanguages();

            // 保存当前选中的筛选条件
            ViewBag.SelectedMusicGenre = musicGenre;
            ViewBag.SelectedLanguage = language;

            return View(products);
        }

        public ActionResult Details(string id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
    }
}