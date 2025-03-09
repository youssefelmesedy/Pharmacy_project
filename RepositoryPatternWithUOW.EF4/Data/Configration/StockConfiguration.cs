
namespace RepositoryPatternWithEFCore.EF4
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.ToTable("Stocks", t =>
                t.HasCheckConstraint("CK_ProductStock_Quantity", "[Quantity] >= 0")
            );

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Quantity)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(s => s.Product)
                   .WithMany(p => p.ProductStocks)
                   .HasForeignKey(s => s.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Warehouse)
                   .WithMany(w => w.ProductStocks)
                   .HasForeignKey(s => s.WarehouseId)
                   .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}