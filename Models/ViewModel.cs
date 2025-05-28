namespace Music_Shopping.Models
{
    public class OrderDetailViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}