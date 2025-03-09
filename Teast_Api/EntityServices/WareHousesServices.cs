namespace Teast_Api.EntityServices
{
    public class WareHousesServices
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly ILogger<WareHousesServices> _logger;
        public readonly IMapper _mapper;

        public WareHousesServices(IUnitOfWork unitOfWork, ILogger<WareHousesServices> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(_unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }

        public async Task<IEnumerable<DtoWareHousesDetials>> GetAllWareHouses()
        {
            try
            {
                var WareHouses = await _unitOfWork.Repository<Warehouse>().GetAllAsync();
                if (WareHouses == null)
                    throw new ArgumentNullException(nameof(WareHouses), "❌ Not Found WareHouses In DataBase..!");

                return _mapper.Map<List<DtoWareHousesDetials>>(WareHouses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while fetching all WareHouses.");
                throw;
            }
        }

        public async Task<DtoWareHousesDetials> GetWareHouseById(int id)
        {
            try
            {
                var warehouse = await _unitOfWork.Repository<Warehouse>().GetByIDAsync(id);
                if (warehouse == null)
                    throw new KeyNotFoundException($"❌ Warehouse with ID {id} not found.");

                return _mapper.Map<DtoWareHousesDetials>(warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while fetching Warehouse with ID {id}.");
                throw;
            }
        }

        public async Task<DtoWareHousesDetials> CreateWareHouse(DtoWareHouses model)
        {
            try
            {
                if (model is null)
                    throw new ArgumentNullException(nameof(model));

                var warehouse = _mapper.Map<Warehouse>(model);
                warehouse = await _unitOfWork.Repository<Warehouse>().CreateAsync(warehouse);
                await _unitOfWork.SaveChangesAsync();

                warehouse = await _unitOfWork.Repository<Warehouse>().GetByIDAsync(warehouse.Id);

                return _mapper.Map<DtoWareHousesDetials>(warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while adding a new Warehouse.");
                throw;
            }
        }

        public async Task<DtoWareHousesDetials> UpdateWareHouse(int Id, DtoWareHouses model)
        {
            try
            {
                if (Id <= 0)
                    throw new ArgumentException($"⚠️ The id must be greater than ({Id}).", nameof(Id));
                var warehouse = await _unitOfWork.Repository<Warehouse>().GetByIDAsync(Id);
                if (warehouse == null)
                    throw new KeyNotFoundException($"❌ Warehouse with ID {Id} not found.");

                _mapper.Map(model, warehouse);
                warehouse = await _unitOfWork.Repository<Warehouse>().UpdateAsync(warehouse);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<DtoWareHousesDetials>(warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while updating Warehouse with ID {Id}.");
                throw;
            }
        }

        public async Task<DtoWareHousesDetials> DeleteWareHouse(int Id)
        {
            try
            {
                if (Id <= 0)
                    throw new ArgumentException($"⚠️ The id must be greater than ({Id}).", nameof(Id));

                var warehouse = await _unitOfWork.Repository<Warehouse>().GetByIDAsync(Id);

                if (warehouse == null)
                    throw new KeyNotFoundException($"❌ Warehouse with ID {Id} not found.");

                warehouse = await _unitOfWork.Repository<Warehouse>().DeleteAsync(warehouse);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<DtoWareHousesDetials>(warehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while deleting Warehouse with ID {Id}.");
                throw;
            }
        }
    }
}
