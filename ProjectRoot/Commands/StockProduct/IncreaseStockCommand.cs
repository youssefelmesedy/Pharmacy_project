namespace ProjectRoot.Commands.StockProduct
{
    public class IncreaseStockCommand : IProductStockCommand
    {
        private readonly StockHandler _stockHandler;
        private readonly ILogger<IncreaseStockCommand> _logger;
        private readonly int _productId;
        private readonly int _warehouseId;
        private readonly int _quantity;

        public IncreaseStockCommand(
            StockHandler stockHandler,
            ILogger<IncreaseStockCommand> logger,
            int productId,
            int warehouseId,
            int quantity)
        {
            _stockHandler = stockHandler ?? throw new ArgumentNullException(nameof(stockHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productId = productId;
            _warehouseId = warehouseId;
            _quantity = quantity;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _logger.LogInformation("🔄 Executing IncreaseStockCommand for ProductId: {ProductId}, WarehouseId: {WarehouseId}, Quantity: {Quantity}",
                                        _productId, _warehouseId, _quantity);

                await _stockHandler.HandleStockAsync(
                    _productId,
                    _warehouseId,
                    _quantity,
                    TransactionTypeEnum.Addition
                );

                _logger.LogInformation("✅ Stock increased successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error while executing IncreaseStockCommand for ProductId: {ProductId}, WarehouseId: {WarehouseId}.", _productId, _warehouseId);
                throw;
            }
        }
    }
}
