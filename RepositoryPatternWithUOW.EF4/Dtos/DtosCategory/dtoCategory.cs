namespace RepositoryPatternWithEFCore.EF4.Dtos.DtosCategory
{
    public class dtoCategory
    {
        [Required(ErrorMessage = "The Category Name Required!")]
        public string? Name { get; set; }
    }
}
