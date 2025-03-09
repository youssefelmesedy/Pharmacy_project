using RepositoryPatternWithUOW.Core;
namespace Teast_Api.ServicesProject
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds application services, repositories, and unit of work to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register All UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register all repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<IStockTransactionService, StockTransactionService>();

            services.AddScoped<CategoryServices>();
            services.AddScoped<ProductServices>();
            services.AddScoped<StockServices>();    
            services.AddScoped<StockTransactionFactory>(); 
            services.AddScoped<WareHousesServices>(); 
            services.AddScoped<StockTransactionServices>(); 

            // ✅ إضافة Logger
            services.AddLogging();

            return services;
        }
    }

}
