namespace RepositoryPatternWithUOW.Core
{
    public interface IStockTransactionService
    {
        Task<StockTransaction> CreateTransactionAsync(StockTransactionDto dto);
        Task<bool> ProcessStockTransactionsAsync(List<StockTransactionDto> transactions);
        Task<IEnumerable<StockTransaction>> GetTransactionsByProductAsync(int productId);
        Task<IEnumerable<StockTransaction>> GetTransactionsByWarehouseAsync(int warehouseId);
        Task<IEnumerable<StockTransaction>> GetAllTransactionsAsync();
    }


}
