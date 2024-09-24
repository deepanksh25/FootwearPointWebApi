namespace FootwearPointWebApi.Models
{
    public class PastOrderViewModel
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
