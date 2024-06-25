using CoreUtilities;

namespace BaseDomain
{
    public interface IBaseRepository<TEntity> where TEntity : BaseModel
    {
        Task<TEntity?> Get(Guid Id);
        Task<Guid> Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(Guid id);
    }

    public abstract class BaseRepository<TEntity>() : IBaseRepository<TEntity> where TEntity : BaseModel
    {
        public Task<TEntity?> Get(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> Create(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
