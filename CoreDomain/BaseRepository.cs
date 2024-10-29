namespace CoreDomain
{
    public interface IBaseRepository<TEntity>
    {
        Task<TEntity?> Get(Guid Id);
        Task<Guid> Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(Guid id);
    }

    public abstract class BaseRepository<TEntity>() : IBaseRepository<TEntity>
    {
        public virtual Task<TEntity?> Get(Guid Id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<Guid> Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
