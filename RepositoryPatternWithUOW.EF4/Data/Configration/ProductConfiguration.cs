namespace RepositoryPatternWithEFCore.EF4
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(55);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.PurchasePrice).HasColumnType("decimal(18,2)");
            builder.Property(p => p.SellingPrice).HasColumnType("decimal(18,2)");
            builder.Property(p => p.DiscountRate).HasColumnType("decimal(18,2)");
            builder.HasIndex(p => p.InternationalCode).IsUnique();
            builder.HasIndex(p => p.ShortCode).IsUnique();
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
