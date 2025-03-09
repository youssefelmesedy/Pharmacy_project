namespace Teast_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryServices _services;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(CategoryServices categoryRepository, IUnitOfWork unitOfWork)
        {
            _services = categoryRepository ?? throw new ArgumentNullException(nameof(_services));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(_unitOfWork));
        }

        // GET: api/<CategoryController>
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> Get()
        {
            var categories = await _services.GetAllCategories();

            return Ok(new
            {
                message = $" ✅ Get All Category (❁´◡`❁) successfully.",
                GetAt = DateTime.UtcNow,
                category = categories
            });
        }

        // GET api/<CategoryController>/5
        [HttpGet("GetByIdCategory{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _services.GetByIDCategories(id);
            return Ok(new
            {
                message = $" ✅ Get Category ById: ({id}) (❁´◡`❁) successfully.",
                GetAt = DateTime.UtcNow,
                Category = category
            });
        }

        // POST api/<CategoryController>
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> Create([FromBody] dtoCategory dtocategory)
        {
            if (dtocategory == null || string.IsNullOrWhiteSpace(dtocategory.Name))
                return BadRequest(new { message = "🚫 Category name is required." });

            var normalizedName = (dtocategory.Name ?? "Null").ToLowerInvariant().Replace(" ", "");

            var existingCategory = await _unitOfWork.Repository<Category>().FindAsync(
                c => (c.Name ?? "Null").ToLower().Replace(" ", "") == normalizedName
            );

            if (existingCategory != null)
                return Conflict(new
                {
                    message = $"⚠️ The category '{existingCategory.Name}' already exists with Id ({existingCategory.Id}).",
                    existingCategory
                });

            var createdCategory = await _services.CreateCategory(dtocategory);

            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, new
            {
                message = $"✅ Category '{createdCategory.Name}' created successfully.",
                AddedAt = DateTime.UtcNow,
                category = createdCategory
            });
        }

        [HttpPut("UpDateCategory{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] dtoCategory dtocategory)
        {
            if (id <= 0)
                return BadRequest(new { message = $" ⚠️ The id must be greater than ({id})." });

            if (dtocategory == null)
                return BadRequest(new { message = $" 🚫 Category data is required. {nameof(dtocategory)}" });

            var existingCategory = await _unitOfWork.Repository<Category>().FindAsync(c => c.Id == id);

            if (existingCategory == null)
                return NotFound(new { message = $" 🚫 The Category With Id: ({id}) Not Existing!", ExistengdAt = DateTime.UtcNow });

            if (!string.Equals(existingCategory.Name, dtocategory.Name, StringComparison.OrdinalIgnoreCase))
            {
                var duplicateCategory = await _unitOfWork.Repository<Category>().FindAsync(
                    c => c.Id != id &&
                    (c.Name ?? "Null").ToLower().Trim().Replace(" ", "")
                    == (dtocategory.Name ?? "Null").ToLower().Trim().Replace(" ", "")
                );

                if (duplicateCategory != null)
                    return Conflict(new
                    {
                        message = $" ⚠️ The Category Name: ({duplicateCategory.Name}) already exists with Id: ({duplicateCategory.Id}).",
                        Conflict = DateTime.UtcNow
                    });
            }

            var updatedCategory = await _services.UpdateCategory(id, dtocategory);

            return Ok(new
            {
                message = $" ✅ Updated Category With Id: ({id}) (❁´◡`❁) successfully.",
                category = updatedCategory,
                updatedAt = DateTime.UtcNow
            });
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("DeleteCategory{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException($" 🚫 The id must be greater than ({id}).", nameof(id));

            var existCategory = await _unitOfWork.Repository<Category>().ExistAsync(c => c.Id == id);
            if (existCategory == true)
            {
                var deleteCategory = await _services.GetByIDCategories(id);

                if (deleteCategory is null)
                    return NotFound($"⚠️ The Category With Id: {id} Not Found");

                await _services.DeleteCategory(deleteCategory);

                return Ok(new
                {
                    Existing = existCategory,
                    message = $" ✅ Deleted Category With ID: ({id}) (❁´◡`❁) successfully.",
                    category = deleteCategory,
                    DeletedAt = DateTime.UtcNow
                });
            }
            else
                return NotFound(new
                {
                    Existing = existCategory,
                    message = $" 🚫 The Category With Id: ({id}) (-:Not Existing(not found)...!:-)",
                    ExistengdAt = DateTime.UtcNow
                });
        }
    }
}
