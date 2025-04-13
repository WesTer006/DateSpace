namespace DataAccessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
