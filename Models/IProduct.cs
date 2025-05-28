using System;

namespace Music_Shopping.Models
{
    public interface IProduct
    {
        string ProductId { get; set; }
        string ProductName { get; set; }
        string Artists { get; set; }
        decimal? Price { get; set; }
        string Type { get; set; }
        string GetDetailViewName();
    }
}