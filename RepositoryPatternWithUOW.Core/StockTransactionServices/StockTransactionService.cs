namespace RepositoryPatternWithUOW.Core
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StockTransactionService> _logger;
        private readonly StockTransactionFactory _transactionFactory;

        public StockTransactionService(IUnitOfWork unitOfWork, ILogger<StockTransactionService> logger, StockTransactionFactory transactionFactory)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _transactionFactory = transactionFactory ?? throw new ArgumentNullException(nameof(transactionFactory));
        }

        public async Task<StockTransaction> CreateTransactionAsync(StockTransactionDto dto)
        {
            try
            {
                var productExists = await _unitOfWork.Repository<Stock>().ExistAsync(p => p.Id == dto.StockId);
                if (!productExists)
                {
                    throw new KeyNotFoundException($"🚫 Product with ID {dto.StockId} does not exist.");
                }
                
                var transaction = _transactionFactory.Create(dto);
                await _unitOfWork.Repository<StockTransaction>().CreateAsync(transaction);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("✅ Stock transaction created successfully for Product ID: {ProductId}", dto.StockId);
                return transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error while creating stock transaction.");
                throw;
            }
        }
        public async Task<bool> ProcessStockTransactionsAsync(List<StockTransactionDto> transactions)
        {
            try
            {
                var stockTransactions = transactions.Select(t => _transactionFactory.Create(t)).ToList();

                await _unitOfWork.Repository<StockTransaction>().CreateListAsync(stockTransactions);

                _logger.LogInformation("✅ {Count} stock transactions processed successfully.", stockTransactions.Count);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error processing stock transactions.");
                return false;
            }
        }

        public async Task<IEnumerable<StockTransaction>> GetTransactionsByProductAsync(int productId)
        {
            return await _unitOfWork.Repository<StockTransaction>().GetAllAsync(t => t.StockId == productId);
        }

        public async Task<IEnumerable<StockTransaction>> GetTransactionsByWarehouseAsync(int warehouseId)
        {
            return await _unitOfWork.Repository<StockTransaction>().GetAllAsync(t => t.DestinationWarehouseId == warehouseId);
        }

        public async Task<IEnumerable<StockTransaction>> GetAllTransactionsAsync()
        {
            return await _unitOfWork.Repository<StockTransaction>().GetAllAsync();
        }
    }

}
