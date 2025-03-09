namespace RepositoryPatternWithEFCore.EF4
{
    public class dtoProduct
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name must not exceed 50 characters.")]
        public string? Pro_Name { get; set; }

        [StringLength(500, ErrorMessage = "Product description must not exceed 500 characters.")]
        public string? Pro_Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0.")]
        public decimal PurchasePrice { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than 0.")]
        public decimal SellingPrice { get; set; }

        public DateTime Pro_DateAdded { get; private set; } = DateTime.Now;

        [Required(ErrorMessage = "Expiry date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime Pro_ExpiryDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "International code must be a positive number.")]
        public int? InternationalCode { get; set; }

        [StringLength(10, ErrorMessage = "Shortcode must not exceed 10 characters.")]
        public string? Pro_Shortcode { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive number.")]
        public int? Pro_CategoryId { get; set; }
    }

}
