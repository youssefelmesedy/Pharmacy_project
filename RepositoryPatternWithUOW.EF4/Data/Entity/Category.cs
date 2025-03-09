namespace RepositoryPatternWithEFCore.EF4
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
        public ICollection<Product>? Products { get; set; } = new List<Product>();
    }
}
