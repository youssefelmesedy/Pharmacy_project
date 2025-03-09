
namespace RepositoryPatternWithEFCore.EF4
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Action)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasComment("The action performed, such as (Add stock, Deduct stock, Transfer stock)");

            builder.Property(a => a.Timestamp)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()")
                   .HasComment("The date and time the action was performed");

            builder.Property(a => a.ReferenceId)
                   .IsRequired(false)
                   .HasComment("The entity ID related to the audit log, can be a product, stock, or transaction ID");

            builder.Property(a => a.StockTransactionId)
                   .IsRequired(false)
                   .HasComment("The stock transaction ID associated with this audit log");

            builder.HasOne(a => a.StockTransaction)
                   .WithMany(st => st.AuditLogs)
                   .HasForeignKey(a => a.StockTransactionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}