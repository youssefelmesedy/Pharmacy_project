namespace RepositoryPatternWithEFCore.EF4
{
    public class DtoStock
    {
        [Required(ErrorMessage = "Stock Name is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Stock Location is required.")]
        public string Location { get; set; } = string.Empty;
    }
}
