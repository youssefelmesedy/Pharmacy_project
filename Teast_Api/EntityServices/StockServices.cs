using RepositoryPatternWithEFCore.EF4.Dtos;
using RepositoryPatternWithUOW.Core;

namespace Teast_Api.EntityServices
{
    public class StockServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StockServices> _logger;
        private readonly IMapper _mapper;
        private readonly IStockTransactionService _stockTransactionService;

        public StockServices(
            IUnitOfWork unitOfWork,
            ILogger<StockServices> logger,
            IMapper mapper,
            IStockTransactionService stockTransactionService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _stockTransactionService = stockTransactionService;
        }

        public async Task<IEnumerable<DtoProductStockDetails>> GetAllProductStock()
        {
            try
            {
                var productsStock = await _unitOfWork.Repository<Stock>().GetAllAsync(p => p.Product!, w => w.Warehouse!);

                if (!productsStock?.Any() ?? true)
                    throw new KeyNotFoundException("No product stock records found in the database.");

                return _mapper.Map<IEnumerable<DtoProductStockDetails>>(productsStock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all product stock records.");
                throw;
            }
        }

        public async Task<DtoProductStockDetails> GetByIDProductStock(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("The product stock ID must be greater than zero.", nameof(id));

                var productStock = await _unitOfWork.Repository<Stock>().GetByIDAsync(id, p => p.Product!, w => w.Warehouse!);

                if (productStock is null)
                    throw new KeyNotFoundException($"No product stock found with ID: {id}.");

                return _mapper.Map<DtoProductStockDetails>(productStock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product stock with ID: {ID}.", id);
                throw;
            }
        }

        public async Task<DtoProductStockDetails> CreateProduct(DtoProductStock dtoProductStock)
        {
            try
            {
                if (dtoProductStock is null)
                    throw new ArgumentNullException(nameof(dtoProductStock), "ProductStock data is required.");

                bool productExists = await _unitOfWork.Repository<Stock>()
                    .ExistAsync(c => c.ProductId == dtoProductStock.ProductId && c.WarehouseId == dtoProductStock.WarehouseId);

                if (productExists)
                    throw new ArgumentException("The product already exists in this stock.", nameof(dtoProductStock.ProductId));

                var createdProduct = await _unitOfWork.Repository<Stock>()
                    .CreateAsync(_mapper.Map<Stock>(dtoProductStock));

                await _unitOfWork.SaveChangesAsync();

                var productDetails = await _unitOfWork.Repository<Stock>()
                    .GetByIDAsync(createdProduct.Id, p => p.Product!, w => w.Warehouse!);

                return _mapper.Map<DtoProductStockDetails>(productDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product in stock.");
                throw;
            }
        }

        public async Task<DtoProductStockDetails> UpdateStock(int id, DtoProductStock dtoProductStock)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("The stock ID must be greater than zero.", nameof(id));

                if (dtoProductStock is null)
                    throw new ArgumentNullException(nameof(dtoProductStock), "Product stock data is required.");

                var existingStock = await _unitOfWork.Repository<Stock>().GetByIDAsync(id);
                if (existingStock is null)
                    throw new ArgumentException("The stock record does not exist.", nameof(id));

                bool productExists = await _unitOfWork.Repository<Stock>()
                    .ExistAsync(c => c.ProductId == dtoProductStock.ProductId &&
                                     c.WarehouseId == dtoProductStock.WarehouseId &&
                                     c.Id != id);

                if (productExists)
                    throw new ArgumentException("The product already exists in this stock.", nameof(dtoProductStock.ProductId));

                _mapper.Map(dtoProductStock, existingStock);
                var updatedStock = await _unitOfWork.Repository<Stock>().UpdateAsync(existingStock);

                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<DtoProductStockDetails>(updatedStock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product stock.");
                throw;
            }
        }

        public async Task<DtoProductStockDetails> DeleteStock(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("The stock ID must be greater than zero.", nameof(id));

                var existingStock = await _unitOfWork.Repository<Stock>().GetByIDAsync(id, p => p.Product!, w => w.Warehouse!);
                if (existingStock is null)
                    throw new ArgumentException("The stock record does not exist.", nameof(id));

                var deletedStockDto = _mapper.Map<DtoProductStockDetails>(existingStock);

                await _unitOfWork.Repository<Stock>().DeleteAsync(existingStock);
                await _unitOfWork.SaveChangesAsync();

                return deletedStockDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product stock.");
                throw;
            }
        }

        
    }
}


