 using System;

namespace Music_Shopping.Models
{
    public abstract class BaseProduct : IProduct
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public string Type { get; set; }

        public string Artists { get; set; }

        public abstract string GetDetailViewName();
    }
}