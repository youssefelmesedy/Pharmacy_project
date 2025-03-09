namespace RepositoryPatternWithUOW.Core
{
    public class StockTransactionFactory
    {
        public StockTransaction Create(StockTransactionDto dto)
        {
            return new StockTransaction
            {
                StockId = dto.StockId,
                DestinationWarehouseId = dto.Action == TransactionTypeEnum.Transfer ? dto.DestinationWarehouseId : null,
                OldValue = dto.OldValue,
                ChangeValue = dto.ValueChanged,
                NewValue = dto.NewValue,
                TransactionType = dto.Action,
                TransactionDate = dto.TransactionDate,
                Description = dto.Description,
                UserId = dto.UserId
            };
        }
    }
}
