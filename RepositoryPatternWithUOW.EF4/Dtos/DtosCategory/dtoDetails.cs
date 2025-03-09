namespace RepositoryPatternWithEFCore.EF4.Dtos.DtosCategory
{
    public class dtoDetails
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreateAt { get; private set; } 
        public List<Product>? Products { get; set; } = new();
    }
}
