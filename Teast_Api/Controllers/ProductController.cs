using Teast_Api.EntityServices;

namespace Teast_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductServices _services;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;
        public ProductController(ProductServices services, IUnitOfWork unitOfWork, ILogger<ProductController> logger)
        {
            _services = services;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/<ValuesController>
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> Get()
        {
            var products = await _services.GetAllProduct();

            return Ok(new
            {
                message = $" ✅ Get All Products (❁´◡`❁) successfully.",
                GetAt = DateTime.UtcNow,
                Products = products
            });
        }
        // GET api/<ValuesController>/5
        [HttpGet("GetByIdProduct{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _services.GetByIDProduct(id);

            return Ok(new
            {
                message = $" ✅ Get Product By Id: ({id}) (❁´◡`❁) successfully.",
                GetAt = DateTime.UtcNow,
                Category = category
            });
        }
        // POST api/<ValuesController>
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> Create([FromBody] dtoProduct dtoProduct)
        {
            if (dtoProduct == null || string.IsNullOrWhiteSpace(dtoProduct.Pro_Name))
                return BadRequest(new { message = "🚫 Product name is required." });

            var normalizedName = (dtoProduct.Pro_Name ?? "Null").ToLowerInvariant().Replace(" ", "");

            var existingProduct = await _unitOfWork.Repository<Product>().FindAsync(c => (c.Name ?? "Null").ToLower().Replace(" ", "") == (dtoProduct.Pro_Name ?? "Null").ToLower().Replace(" ", "")
                );

            if (existingProduct != null)
                return Conflict(new
                {
                    message = $"⚠️ Product '{existingProduct.Name}' already exists with Id ({existingProduct.Id}).",
                    Product = existingProduct
                });

            var createdProduct = await _services.CreateProduct(dtoProduct);

            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, new
            {
                message = $"✅ Product '{createdProduct.pro_Name}' created successfully.",
                AddedAt = DateTime.UtcNow,
                Product = createdProduct
            });
        }
        // PUT api/<ValuesController>/5
        [HttpPut("UpDateProduct{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] dtoProduct dtoProduct)
        {
            if (id <= 0)
                return BadRequest(new { message = $" ⚠️ The id must be greater than Id: ({id})." });

            if (dtoProduct == null)
                return BadRequest(new { message = $" 🚫 Product data is required NameOf: ({nameof(dtoProduct)})." });

            // التحقق من وجود المنتج
            var existingProduct = await _unitOfWork.Repository<Product>().GetByIDAsync(id);
            if (existingProduct == null)
                return NotFound(new { message = $" 🚫 The Product with Id: ({id}) does not exist.", CheckedAt = DateTime.UtcNow });

            if (!string.Equals(existingProduct.Name, dtoProduct.Pro_Name, StringComparison.OrdinalIgnoreCase))
            {
                var productWithSameName = await _unitOfWork.Repository<Product>().FindAsync(
                    c => c.Id != id &&
                    (c.Name ?? "Null").ToLower().Replace(" ", "") == (dtoProduct.Pro_Name ?? "Null").ToLower().Replace(" ", "")
                );

                if (productWithSameName != null)
                    return Conflict(new
                    {
                        message = $" ⚠️ The Product Name: ({productWithSameName.Name}) already exists with Id: ({productWithSameName.Id}).",
                        Conflict = DateTime.UtcNow,
                        existingProduct = productWithSameName
                    });
            }
            var updatedProduct = await _services.UpDateProduct(id, dtoProduct);
            return Ok(new
            {
                message = $" ✅ Updated Product with Id: ({id}) successfully.",
                updatedAt = DateTime.UtcNow,
                Product = updatedProduct
            });
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("DeleteProduct{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException($" 🚫 The id must be greater than ({id}).", nameof(id));

            var existProduct = await _unitOfWork.Repository<Product>().ExistAsync(c => c.Id == id);
            if (existProduct == true)
            {
                var deleteProduct = await _services.GetByIDProduct(id);

                if (deleteProduct is null)
                    return NotFound($"⚠️ The Product With Id: {id} Not Found");

                await _services.DeleteProduct(deleteProduct);

                return Ok(new
                {
                    Existing = existProduct,
                    message = $" ✅ Deleted Product With ID: ({id}) (❁´◡`❁) successfully.",
                    category = deleteProduct,
                    DeletedAt = DateTime.UtcNow
                });
            }
            else
                return NotFound(new
                {
                    Existing = existProduct,
                    message = $" 🚫 The Product With Id: ({id}) (-:Not Existing(not found)...!:-)",
                    ExistengdAt = DateTime.UtcNow
                });
        }
        
    }
}
