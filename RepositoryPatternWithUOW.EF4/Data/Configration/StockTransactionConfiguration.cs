namespace RepositoryPatternWithEFCore.EF4
{
    public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> builder)
        {
            builder.ToTable("StockTransactions");

            builder.HasKey(st => st.Id);

            builder.Property(st => st.OldValue).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(st => st.ChangeValue).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(st => st.NewValue).HasColumnType("decimal(18,2)").IsRequired();

            builder.ToTable(t => t.HasCheckConstraint("CK_StockTransaction_Values", "[NewValue] = [OldValue] + [ChangeValue]"));

            builder.Property(st => st.TransactionType)
                   .HasConversion<string>();


            builder.HasOne(st => st.ProductStock)
                   .WithMany(s => s.StockTransactions)
                   .HasForeignKey(st => st.StockId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(st => st.DestinationWarehouse)
                   .WithMany(w => w.StockTransactions)
                   .HasForeignKey(st => st.DestinationWarehouseId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasMany(st => st.AuditLogs)
                   .WithOne(al => al.StockTransaction)
                   .HasForeignKey(al => al.ReferenceId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}