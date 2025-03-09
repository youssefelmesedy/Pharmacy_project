namespace ProjectRoot.Commands.StockProduct
{
    public class DecreaseStockCommand : IProductStockCommand
    {
        private readonly StockHandler _stockHandler;
        private readonly ILogger<DecreaseStockCommand> _logger;
        private readonly int _productId;
        private readonly int _warehouseId;
        private readonly int _quantity;

        public DecreaseStockCommand(
            StockHandler stockHandler,
            ILogger<DecreaseStockCommand> logger,
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
                _logger.LogInformation("🔄 Executing DecreaseStockCommand for ProductId: {ProductId}, WarehouseId: {WarehouseId}, Quantity: {Quantity}",
                                        _productId, _warehouseId, _quantity);

                await _stockHandler.HandleStockAsync(
                    _productId,
                    _warehouseId,
                    _quantity,
                    TransactionTypeEnum.Deduction
                );

                _logger.LogInformation("✅ Stock decreased successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error while executing DecreaseStockCommand for ProductId: {ProductId}, WarehouseId: {WarehouseId}.", _productId, _warehouseId);
                throw;
            }
        }
    }

}
