namespace FootwearPointWebApi.Models
{
    public class ShoeViewModel
    {
        public int ProductID { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantityInStock { get; set; }
        public string ProductImageUrl { get; set; }
        public int ProductSize { get; set; }
        public string GenderCategory { get; set; }
        public byte[] ProductImage { get; set; } = null;// Assuming it's a binary image
        public int ProductSubCategoryID { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductColor { get; set; }
        public int ProductQuantityAdded { get; set; }
    }
}
