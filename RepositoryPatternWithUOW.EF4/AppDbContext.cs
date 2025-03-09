namespace RepositoryPatternWithEFCore.EF4
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Stock> stock { get; set; }
        public DbSet<Warehouse> warehouses { get; set; }
        public DbSet<AuditLog> auditLog { get; set; }
        public DbSet<StockTransaction> stockTransaction { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
