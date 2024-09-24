using System.ComponentModel.DataAnnotations;

namespace FootwearPointWebApi.Models
{
    public class ProductViewModel
    {
        [Display(Name = "Product ID")]
        [Required]
        public int ProductID { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        [MaxLength(50)]
        //[Remote("checkduplicate","product","ProductName",HttpMethod="GET")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Product Category ID")]
        public int ProductCategoryID { get; set; }
        [Display(Name = "Product Subcategory ID")]
        [Required]
        public int ProductSubCategoryID { get; set; }
        [Required]
        [Display(Name = "Product Color")]
        public string ProductColor { get; set; }
        [Required]
        [Range(50, 20000)]
        [Display(Name = "Product Price")]
        public int ProductPrice { get; set; }
        //[DataType(DataType.ImageUrl)]
        [Required]
        [Display(Name = "Image")]
        public string ProductImageUrl { get; set; } = "";
        [Required]
        [Display(Name = "Product Quantity Left")]
        public int ProductQuantityLeft { get; set; }
        [Required]
        [Range(3, 13)]
        [Display(Name = "Product Size")]
        public int ProductSize { get; set; }
        [Required]
        [Display(Name = "Gender Category")]
        public string GenderCategory { get; set; }
        public IFormFile ProductImage { get; set; } = null;
        [Required]
        [Range(1, 100)]
        public int ProductQuantityAdded { get; set; } = 0;


    }

}
