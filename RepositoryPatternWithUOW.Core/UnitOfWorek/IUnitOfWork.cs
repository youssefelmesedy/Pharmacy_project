namespace RepositoryPatternWithUOW.Core.UnitOfWorek
{
    public interface IUnitOfWork: IDisposable
    {
        IBaseRepository<T> Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
        Task ExecuteTransactionAsync(Func<Task> action);
    }
}
