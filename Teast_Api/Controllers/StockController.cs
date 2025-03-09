namespace Teast_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockServices _services;
        private readonly ILogger<StockController> _logger;

        public StockController(StockServices services, ILogger<StockController> logger)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // ✅ Get All Product Stocks
        [HttpGet("GetAllStock")]
        public async Task<IActionResult> GetAllStock()
        {
            try
            {
                var products = await _services.GetAllProductStock();
                return Ok(new
                {
                    message = "✅ Retrieved all product stock successfully.",
                    retrievedAt = DateTime.UtcNow,
                    data = products
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while retrieving all product stock.");
                return StatusCode(500, new { message = "❌ Internal Server Error.", error = ex.Message });
            }
        }

        // ✅ Get Product Stock By ID
        [HttpGet("GetByIdStock/{id}")]
        public async Task<IActionResult> GetByIdStock(int id)
        {
            try
            {
                var productStock = await _services.GetByIDProductStock(id);
                return Ok(new
                {
                    message = $"✅ Product stock with ID: {id} retrieved successfully.",
                    retrievedAt = DateTime.UtcNow,
                    data = productStock
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"🚫 Product stock with ID: {id} not found.");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while retrieving product stock with ID: {id}.");
                return StatusCode(500, new { message = "❌ Internal Server Error.", error = ex.Message });
            }
        }

        // ✅ Create New Product Stock
        [HttpPost("CreateStock")]
        public async Task<IActionResult> CreateStock([FromBody] DtoProductStock dtoProductStock)
        {
            try
            {
                var createdStock = await _services.CreateProduct(dtoProductStock);
                return CreatedAtAction(nameof(GetByIdStock), new { id = createdStock.Id }, new
                {
                    message = "✅ Product stock created successfully.",
                    createdAt = DateTime.UtcNow,
                    data = createdStock
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "⚠️ Invalid input while creating product stock.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while creating product stock.");
                return StatusCode(500, new { message = "❌ Internal Server Error.", error = ex.Message });
            }
        }

        // ✅ Update Product Stock
        [HttpPut("UpdateStock/{id}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] DtoProductStock dtoProductStock)
        {
            try
            {
                var updatedStock = await _services.UpdateStock(id, dtoProductStock);
                return Ok(new
                {
                    message = $"✅ Product stock with ID: {id} updated successfully.",
                    updatedAt = DateTime.UtcNow,
                    data = updatedStock
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"🚫 Product stock with ID: {id} not found.");
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "⚠️ Invalid input while updating product stock.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while updating product stock with ID: {id}.");
                return StatusCode(500, new { message = "❌ Internal Server Error.", error = ex.Message });
            }
        }

        // ✅ Delete Product Stock
        [HttpDelete("DeleteStock/{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            try
            {
                var deletedStock = await _services.DeleteStock(id);
                return Ok(new
                {
                    message = $"✅ Product stock with ID: {id} deleted successfully.",
                    deletedAt = DateTime.UtcNow,
                    data = deletedStock
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"🚫 Product stock with ID: {id} not found.");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error occurred while deleting product stock with ID: {id}.");
                return StatusCode(500, new { message = "❌ Internal Server Error.", error = ex.Message });
            }
        }
    }
}
