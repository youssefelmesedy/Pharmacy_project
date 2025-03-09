namespace ProjectRoot.Commands.StockProduct
{
    public class TransferStockCommand : IProductStockCommand
    {
        private readonly StockHandler _stockHandler;
        private readonly ILogger<TransferStockCommand> _logger;
        private readonly int _productId;
        private readonly int _sourceWarehouseId;
        private readonly int _destinationWarehouseId;
        private readonly int _quantity;

        public TransferStockCommand(
            StockHandler stockHandler,
            ILogger<TransferStockCommand> logger,
            int productId,
            int sourceWarehouseId,
            int destinationWarehouseId,
            int quantity)
        {
            _stockHandler = stockHandler ?? throw new ArgumentNullException(nameof(stockHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productId = productId;
            _sourceWarehouseId = sourceWarehouseId;
            _destinationWarehouseId = destinationWarehouseId;
            _quantity = quantity;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _logger.LogInformation("🔄 Executing TransferStockCommand for ProductId: {ProductId}, SourceWarehouse: {SourceWarehouseId}, DestinationWarehouse: {DestinationWarehouseId}, Quantity: {Quantity}",
                                        _productId, _sourceWarehouseId, _destinationWarehouseId, _quantity);

                await _stockHandler.HandleStockAsync(
                    _productId,
                    _sourceWarehouseId,
                    _quantity,
                    TransactionTypeEnum.Transfer,
                    _destinationWarehouseId
                );

                _logger.LogInformation("✅ Stock transferred successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error while executing TransferStockCommand for ProductId: {ProductId}.", _productId);
                throw;
            }
        }
    }

}
