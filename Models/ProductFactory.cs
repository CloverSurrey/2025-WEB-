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

                var baseProduct = _db.products.FirstOrDefault(p => p.product_id == productId);

                // 检查产品类型是否包含"RCD"（不区分大小写）
                if (baseProduct.type?.IndexOf("RCD", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var recordDetail = _db.products_records.FirstOrDefault(r => r.product_id == productId);
                    if (recordDetail == null)
                    {
                        return null;
                    }

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

                return null;
        }
    }
}