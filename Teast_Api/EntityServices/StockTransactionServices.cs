using RepositoryPatternWithEFCore.EF4.Dtos;

namespace Teast_Api.EntityServices
{
    public class StockTransactionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<StockTransactionServices> _logger;

        public StockTransactionServices(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StockTransactionServices> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// جلب جميع المعاملات المخزنية.
        /// </summary>
        public async Task<IEnumerable<StockTransactionDto>> GetAllStockTransactionsAsync()
        {
            try
            {
                var transactions = await _unitOfWork.Repository<StockTransaction>().GetAllAsync();
                if (transactions == null || !transactions.Any())
                {
                    _logger.LogWarning("⚠️ No stock transactions found in the database.");
                    return Enumerable.Empty<StockTransactionDto>();
                }

                return _mapper.Map<IEnumerable<StockTransactionDto>>(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while fetching stock transactions.");
                throw;
            }
        }

        /// <summary>
        /// جلب معاملة مخزنية حسب المعرف.
        /// </summary>
        public async Task<StockTransactionDto> GetStockTransactionByIdAsync(int transactionId)
        {
            try
            {
                var transaction = await _unitOfWork.Repository<StockTransaction>().GetByIDAsync(transactionId);
                if (transaction == null)
                    throw new KeyNotFoundException($"❌ Stock transaction with ID {transactionId} not found.");

                return _mapper.Map<StockTransactionDto>(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while fetching stock transaction with ID {transactionId}.");
                throw;
            }
        }

        /// <summary>
        /// جلب جميع المعاملات المخزنية الخاصة بمنتج معين.
        /// </summary>
        public async Task<IEnumerable<StockTransactionDto>> GetTransactionsByProductAsync(int productId)
        {
            try
            {
                var transactions = await _unitOfWork.Repository<StockTransaction>()
                                   .GetAllAsync();

                var filteredTransactions = transactions
                   .Where(t => t.Id == productId)
                   .ToList();

                if (filteredTransactions is null)
                {
                    _logger.LogWarning($"⚠️ No transactions found for product ID {productId}.");
                    return Enumerable.Empty<StockTransactionDto>();
                }

                return _mapper.Map<IEnumerable<StockTransactionDto>>(filteredTransactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while fetching transactions for product ID {productId}.");
                throw;
            }
        }

        /// <summary>
        /// جلب جميع المعاملات المخزنية الخاصة بمستودع معين.
        /// </summary>
        public async Task<IEnumerable<StockTransactionDto>> GetTransactionsByWarehouseAsync(int warehouseId)
        {
            try
            {
                var transactions = await _unitOfWork.Repository<StockTransaction>()
                                   .GetAllAsync();

                var filteredTransactions = transactions
                    .Where(t => t.DestinationWarehouseId == warehouseId )
                    .ToList();

                if (!filteredTransactions.Any())
                {
                    _logger.LogWarning($"⚠️ No transactions found for warehouse ID {warehouseId}.");
                    return Enumerable.Empty<StockTransactionDto>();
                }

                return _mapper.Map<IEnumerable<StockTransactionDto>>(filteredTransactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while fetching transactions for warehouse ID {warehouseId}.");
                throw;
            }
        }

        /// <summary>
        /// جلب جميع المعاملات المخزنية خلال فترة زمنية محددة.
        /// </summary>
        public async Task<IEnumerable<StockTransactionDto>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = await _unitOfWork.Repository<StockTransaction>().GetAllAsync(); 

                var filteredTransactions = transactions
                    .Where(t => t.TransactionDate.Date >= startDate.Date && t.TransactionDate.Date <= endDate.Date)
                    .ToList(); 

                if (!filteredTransactions.Any())
                {
                    _logger.LogWarning($"⚠️ No transactions found between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}.");
                    return Enumerable.Empty<StockTransactionDto>();
                }

                return _mapper.Map<IEnumerable<StockTransactionDto>>(filteredTransactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while fetching transactions between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}.");
                throw;
            }
        }

    }
}
