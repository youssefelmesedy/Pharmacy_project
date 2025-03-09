namespace RepositoryPatternWithEFCore.EF4.Dtos.DtoProduct
{
    public class DtoDetailsProduct
    {
        public int Id { get; set; }
        public string? pro_Name { get; set; }
        public string? pro_Description { get; set; }
        public decimal Purchaseprice { get; set; }
        public decimal sellingprice { get; set; }
        public DateTime Pro_DateAdded { get; private set; } = DateTime.Now;
        public DateTime Pro_Expirydate { get; set; }
        public bool productExpiry { get; set; }
        public int? InternationalCode { get; set; }
        public string? Pro_Shortcode { get; set; }
        public int? pro_CategoryId { get; set; }
        public string? Category_Name { get; set; }
    }
}
