namespace RepositoryPatternWithEFCore.EF4.Dtos.DtoWareHouses
{
    public class DtoWareHouses
    {
        [Required(ErrorMessage = "WareHouses name is required.")]
        [StringLength(100, ErrorMessage = "WareHouses name must not exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "WareHouses name is required.")]
        [StringLength(100, ErrorMessage = "WareHouses name must not exceed 50 characters.")]
        public string Location { get; set; } = string.Empty;
    }
    public class DtoWareHousesDetials
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
