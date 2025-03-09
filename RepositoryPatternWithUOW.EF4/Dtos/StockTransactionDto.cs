using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithEFCore.EF4.Dtos
{
    public class StockTransactionDto
    {
        public int StockId { get; set; }
        public int? DestinationWarehouseId { get; set; }
        public TransactionTypeEnum Action { get; set; }
        public DateTime TransactionDate { get; set; }   
        public decimal OldValue { get; set; }
        public decimal ValueChanged { get; set; }
        public decimal NewValue { get; set; }
        public string? Description { get; set; }
        public int? UserId { get; set; }
    }

}
