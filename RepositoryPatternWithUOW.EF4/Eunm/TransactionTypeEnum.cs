namespace RepositoryPatternWithEFCore.EF4.Eunm
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionTypeEnum
    {
        Purchase,
        Sale,
        Transfer,
        Adjustment
    }
}
