namespace RepositoryPatternWithUOW.EF4.EntityServices
{
    public class CategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryServices> _logger;

        public CategoryServices(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryServices> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentException(nameof(_unitOfWork));
            _logger = logger ?? throw new ArgumentException(nameof(_logger));
            _mapper = mapper ?? throw new ArgumentException(nameof(_mapper));
        }
        public async Task<List<dtoDetails>> GetAllCategories()
        {
            try
            {
                var categories = await _unitOfWork.Repository<Category>().GetAllAsync(p => p.Products!);

                if (categories == null || !categories.Any())
                {
                    throw new KeyNotFoundException("🚫 No categories available in the database.");
                }

                return _mapper.Map<List<dtoDetails>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while fetching all categories.");
                throw;
            }
        }
        public async Task<dtoDetails> GetByIDCategories(int Id)
        {
            try
            {
                if (Id <= 0)
                    throw new ArgumentException($"⚠️ The id must be greater than ({Id}).", nameof(Id));
                // Get By ID categories from the repository, including their products.
                var categories = await _unitOfWork.Repository<Category>().GetByIDAsync(Id, p => p.Products!);

                // Check if we got any categories.
                if (categories == null)
                {
                    throw new ArgumentNullException("🚫 No categories available in the database.", nameof(categories));
                }

                return _mapper.Map<dtoDetails>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while fetching category With Id: ({Id}).");
                throw;
            }
        }
        public async Task<dtoDetails> CreateCategory(dtoCategory dtocategory)
        {
            try
            {
                if (dtocategory == null)
                    throw new ArgumentException("⚠️ Category data is required.", nameof(dtocategory));

                // If category doesn't exist, create it
                var category = _mapper.Map<Category>(dtocategory);
                await _unitOfWork.Repository<Category>().CreateAsync(category);
                await _unitOfWork.SaveChangesAsync();

                // Return the newly created category with a success message
                return _mapper.Map<dtoDetails>(category);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while creating category.");
                throw;
            }
        }
        public async Task<dtoDetails> UpdateCategory(int Id, dtoCategory dtocategory)
        {
            try
            {
                if (Id <= 0)
                    throw new ArgumentOutOfRangeException($"⚠️ The id must be greater than ({Id}).", nameof(Id));

                if (dtocategory == null)
                    throw new ArgumentException("⚠️ Category data is required.", nameof(dtocategory));

                var getCategory = await _unitOfWork.Repository<Category>().GetByIDAsync(Id, p => p.Products!);
                if (getCategory is null)
                    throw new ArgumentNullException($"🚫 No categories available in the database. With ID: ({Id})");

                _mapper.Map(dtocategory, getCategory);

                await _unitOfWork.Repository<Category>().UpdateAsync(getCategory);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<dtoDetails>(getCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while UpDate category With Id: ({Id}).");
                throw;
            }
        }
        public async Task<dtoDetails> DeleteCategory(dtoDetails dtoDetails)
        {
            try
            {

                if (dtoDetails == null)
                    throw new ArgumentException("⚠️ Category data is required.", nameof(dtoDetails));

               var category =  await _unitOfWork.Repository<Category>().DeleteAsync(_mapper.Map<Category>(dtoDetails));
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<dtoDetails>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while Delete category With Id: ({dtoDetails.Id}).");
                throw;
            }
        }
    }
}
