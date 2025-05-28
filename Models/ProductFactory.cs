using System;
using System.Linq;
using System.Diagnostics;

namespace Music_Shopping.Models
{
    public class ProductFactory
    {
        private readonly Music_ShoppingEntities _db;

        public ProductFactory(Music_ShoppingEntities db)
        {
            _db = db;
        }

        public IProduct CreateProduct(string productId)
        {
            try
            {
                var baseProduct = _db.products.FirstOrDefault(p => p.product_id == productId);
                if (baseProduct == null)
                {
                    Debug.WriteLine($"未找到基础产品信息: {productId}");
                    return null;
                }

                Debug.WriteLine($"找到基础产品: ID={baseProduct.product_id}, 名称={baseProduct.product_name}, 类型={baseProduct.type}");

                // 检查产品类型是否包含"record"（不区分大小写）
                if (baseProduct.type?.IndexOf("RCD", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var recordDetail = _db.products_records.FirstOrDefault(r => r.product_id == productId);
                    if (recordDetail == null)
                    {
                        Debug.WriteLine($"未找到唱片详细信息: {productId}");
                        return null;
                    }

                    Debug.WriteLine($"找到唱片详细信息: ID={recordDetail.product_id}, 语言={recordDetail.language}, 发行日期={recordDetail.release_date}");

                    return new RecordProduct
                    {
                        ProductId = baseProduct.product_id,
                        ProductName = baseProduct.product_name,
                        Price = baseProduct.price,
                        Type = baseProduct.type,
                        Artists = baseProduct.artists,
                        ReleaseDate = recordDetail.release_date.Value.ToString("yyyy-MM-dd"),
                        Language = recordDetail.language,
                        MusicGenre = recordDetail.music_genre
                    };
                }

                Debug.WriteLine($"未知的产品类型: {baseProduct.type}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"创建产品时发生错误: {ex.Message}");
                Debug.WriteLine($"错误详情: {ex.StackTrace}");
                return null;
            }
        }
    }
}