using RepositoryPatternWithEFCore.EF4.Dtos;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.EF4;

namespace Teast_Api.EntityServices
{
    public class ProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductServices> _logger;
        private readonly IStockTransactionService _stockTransactionService;
        public ProductServices(IUnitOfWork unitOfWork, ILogger<ProductServices> logger, IMapper mapper, IStockTransactionService stockTransactionService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(_unitOfWork));
            _logger = logger ?? throw new ArgumentException(nameof(_logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(_mapper));
            _stockTransactionService = stockTransactionService;
        }

        public async Task<IEnumerable<DtoDetailsProduct>> GetAllProduct()
        {
            try
            {
                var products = await _unitOfWork.Repository<Product>().GetAllAsync(c => c.Category!);

                if (products == null || !products.Any())
                {
                    throw new KeyNotFoundException("🚫 No Product available in the database.");
                }

                return _mapper.Map<IEnumerable<DtoDetailsProduct>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while fetching all Products.");
                throw;
            }
        } 
        public async Task<DtoDetailsProduct> GetByIDProduct(int Id)
        {
            try
            {
                if (Id <= 0)
                    throw new ArgumentException($"⚠️ The id must be greater than ({Id}).", nameof(Id));
                // Get Product By Id from the repository, including their products.
                var product = await _unitOfWork.Repository<Product>().GetByIDAsync(Id, p => p.Category!);

                // Check if we got any categories.
                if (product == null)
                {
                    throw new ArgumentNullException("🚫 No Product available in the database.", nameof(product));
                }

                return _mapper.Map<DtoDetailsProduct>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while fetching Product With Id: ({Id}).");
                throw;
            }
        }
        public async Task<DtoDetailsProduct> CreateProduct(dtoProduct dtoProduct)
        {
            try
            {
                if (dtoProduct == null)
                    throw new ArgumentException("⚠️ Product data is required.", nameof(dtoProduct));

                var exsitCategoryId = await _unitOfWork.Repository<Category>().ExistAsync(c => c.Id == dtoProduct.Pro_CategoryId);
                if(exsitCategoryId == false)
                    throw new ArgumentException("🚫 No Category available in the database.", nameof(exsitCategoryId));

                var exsitInternationalCode = await _unitOfWork.Repository<Product>().ExistAsync(c => c.InternationalCode == dtoProduct.InternationalCode);
                if (exsitInternationalCode == true)
                    throw new ArgumentException("⚠️ InternationalCode already exists in Product.", nameof(exsitInternationalCode));

                var exsitPro_Shortcode = await _unitOfWork.Repository<Product>().ExistAsync(c => c.ShortCode == dtoProduct.Pro_Shortcode);
                if (exsitPro_Shortcode == true)
                    throw new ArgumentException("⚠️ Pro_Shortcode already exists in Product.", nameof(exsitPro_Shortcode));

                var createproduct = await _unitOfWork.Repository<Product>().CreateAsync(_mapper.Map<Product>(dtoProduct));
                await _unitOfWork.SaveChangesAsync();

                var product = await _unitOfWork.Repository<Product>().GetByIDAsync(createproduct.Id, c => c.Category!);
                // Return the newly created Product  with a success message
                return _mapper.Map<DtoDetailsProduct>(product);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while creating Product.");
                throw;
            }
        }
        public async Task<DtoDetailsProduct> UpDateProduct(int Id,dtoProduct dtoProduct)
        {
            try
            {
                if (Id <= 0)
                    throw new ArgumentException($"⚠️ The id must be greater than ({Id}).", nameof(Id));

                if (dtoProduct == null)
                    throw new ArgumentException("⚠️ Product data is required.", nameof(dtoProduct));

                var exsitCategoryId = await _unitOfWork.Repository<Category>().ExistAsync(c => c.Id == dtoProduct.Pro_CategoryId);
                if (exsitCategoryId == false)
                    throw new ArgumentException("🚫 No Category available in the database.", nameof(exsitCategoryId));

                var exsitInternationalCode = await _unitOfWork.Repository<Product>().ExistAsync(c => c.InternationalCode == dtoProduct.InternationalCode && c.Id != Id);
                if (exsitInternationalCode == true)
                    throw new ArgumentException("⚠️ InternationalCode already exists in Product.", nameof(exsitInternationalCode));

                var exsitPro_Shortcode = await _unitOfWork.Repository<Product>().ExistAsync(c => c.ShortCode == dtoProduct.Pro_Shortcode && c.Id != Id);
                if (exsitPro_Shortcode == true)
                    throw new ArgumentException("⚠️ Pro_Shortcode already exists in Product.", nameof(exsitPro_Shortcode));

                var product = await _unitOfWork.Repository<Product>().GetByIDAsync(Id, c => c.Category!);

                var createproduct = await _unitOfWork.Repository<Product>().UpdateAsync(_mapper.Map(dtoProduct,product));
                await _unitOfWork.SaveChangesAsync();

                // Return the newly created Product  with a success message
                return _mapper.Map<DtoDetailsProduct>(product);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while UpDateing Product.");
                throw;
            }
        }
        public async Task<DtoDetailsProduct> DeleteProduct(DtoDetailsProduct dtoProduct)
        {
            try
            {

                if (dtoProduct == null)
                    throw new ArgumentException("⚠️ Category data is required.", nameof(dtoProduct));

                var product = await _unitOfWork.Repository<Product>().DeleteAsync(_mapper.Map<Product>(dtoProduct));
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<DtoDetailsProduct>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while Delete category With Id: ({dtoProduct.Id}).");
                throw;
            }
        }
       
    }
}
