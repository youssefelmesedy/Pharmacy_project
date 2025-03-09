namespace RepositoryPatternWithEFCore.EF4
{
    public class DtoProductStock
    {
        [Required(ErrorMessage = "Product Shortcod is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Warehouse ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Warehouse ID must be greater than 0.")]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater.")]
        public decimal Quantity { get; set; }
    }
    public class DtoProductStockDetails
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime Expiry { get; set; }
        public int WarehouseId { get; set; }
        public string? WarehouseName { get; set; }
        public decimal Quantity { get; set; }
    }
}
