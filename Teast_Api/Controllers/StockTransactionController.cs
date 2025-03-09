namespace Teast_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTransactionController : ControllerBase
    {
        private readonly StockTransactionServices _stockTransactionServices;

        public StockTransactionController(StockTransactionServices stockTransactionServices)
        {
            _stockTransactionServices = stockTransactionServices ?? throw new ArgumentNullException(nameof(stockTransactionServices));
        }

        /// <summary>
        /// جلب جميع المعاملات المخزنية مع دعم التصفّح (Pagination).
        /// </summary>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var transactions = await _stockTransactionServices.GetAllStockTransactionsAsync();
            var pagedTransactions = transactions.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return Ok(pagedTransactions);
        }

        /// <summary>
        /// جلب معاملة مخزنية حسب معرفها.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _stockTransactionServices.GetStockTransactionByIdAsync(id);
            return Ok(transaction);
        }

        /// <summary>
        /// جلب جميع المعاملات الخاصة بمنتج معين.
        /// </summary>
        [HttpGet("ByProduct/{productId}")]
        public async Task<IActionResult> GetTransactionsByProduct(int productId)
        {
            var transactions = await _stockTransactionServices.GetTransactionsByProductAsync(productId);
            return Ok(transactions);
        }

        /// <summary>
        /// جلب جميع المعاملات الخاصة بمستودع معين.
        /// </summary>
        [HttpGet("ByWarehouse/{warehouseId}")]
        public async Task<IActionResult> GetTransactionsByWarehouse(int warehouseId)
        {
            var transactions = await _stockTransactionServices.GetTransactionsByWarehouseAsync(warehouseId);
            return Ok(transactions);
        }

        /// <summary>
        /// جلب جميع المعاملات خلال فترة زمنية محددة.
        /// </summary>
        [HttpGet("ByDateRange")]
        public async Task<IActionResult> GetTransactionsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var transactions = await _stockTransactionServices.GetTransactionsByDateRangeAsync(startDate, endDate);
            return Ok(transactions);
        }
    }

}
