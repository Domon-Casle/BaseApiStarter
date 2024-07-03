using CoreDomain.Audit;
using CoreUtilities;
using CoreUtilities.Logger;
using System.Collections.Concurrent;

namespace CoreDomain
{
    public interface IBaseDomain<TEntity, TRepo> where TEntity : BaseModel where TRepo : IBaseRepository<TEntity>
    {
        Task<TEntity?> Get(Guid Id);
        Task<Guid> Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(Guid id);
    }

    public abstract class BaseDomain<TEntity, TRepo> where TEntity : BaseModel where TRepo : IBaseRepository<TEntity>
    {
        private static ConcurrentDictionary<Type, TableCache> GlobalTableCache = new ConcurrentDictionary<Type, TableCache>();
        protected readonly TRepo _repository;
        protected readonly IUserPrincipal User;
        protected readonly IBaseLogger Logger;
        protected readonly IAuditDomain _auditDomain;
        protected readonly TableCache TableDetails;

        public BaseDomain(TRepo repo, IUserPrincipal userPrincapal, IBaseLogger logger, IAuditDomain auditDomain)
        {
            _repository = repo;
            User = userPrincapal;
            Logger = logger;
            _auditDomain = auditDomain;

            TableDetails = GlobalTableCache.GetOrAdd(typeof(TEntity), new TableCache(typeof(TEntity)));
        }

        public virtual async Task<TEntity?> Get(Guid id)
        {
            Require.NotEmpty(id, nameof(id));

            Logger.LogDebug($"Finding {id} for entity {TableDetails.Name}");
            var entity = await _repository.Get(id);
#pragma warning disable CS8604 // Possible null reference argument.
            Require.NotNull(entity, nameof(entity));
#pragma warning restore CS8604 // Possible null reference argument.

            if (TableDetails.LogRead)
            {
                Logger.LogDebug($"Read of {TableDetails.Name} with id {id} sent to external Logger");
                await _auditDomain.AuditRead(id, User.UserId, TableDetails);
            }
            return entity;
        }

        public async Task<Guid> Create(TEntity entity)
        {
            Require.NotNull(entity, nameof(entity));

            entity.CreatedBy = User.UserId;
            entity.CreatedOn = DateTime.UtcNow;
            entity.LastModifiedBy = User.UserId;
            entity.LastModifiedOn = DateTime.UtcNow;

            Logger.LogDebug($"Creating {TableDetails.Name}", entity);
            var newId = await _repository.Create(entity);
            Logger.LogDebug($"Created {TableDetails.Name} with id {newId}");

            if (TableDetails.LogCreate)
            {
                Logger.LogDebug($"Creation of {TableDetails.Name} with id {newId} sent to external Logger");
                await _auditDomain.AuditCreate(entity, TableDetails);
            }
            return newId;
        }

        public async Task Update(TEntity entity)
        {
            Require.NotNull(entity, nameof(entity));

            var oldEntity = await Get(entity.Id);
            var variances = _auditDomain.AreThereChanges(oldEntity, entity, TableDetails);
            if (variances.Count > 0)
            {
                entity.LastModifiedBy = User.UserId;
                entity.LastModifiedOn = DateTime.UtcNow;

                Logger.LogDebug($"Updating {TableDetails.Name}", entity);
                await _repository.Update(entity);
                Logger.LogDebug($"Updated {TableDetails.Name} with id {entity.Id}");

                if (TableDetails.LogUpdate)
                {
                    Logger.LogDebug($"Update of {TableDetails.Name} with id {entity.Id} sent to external Logger");
                    await _auditDomain.AuditChanges(entity, User.UserId, variances, TableDetails);
                }
            }
            else
            {
                Logger.LogDebug($"No differences found no update done for id {entity.Id}");
            }
        }

        public async Task Delete(Guid id)
        {
            Require.NotEmpty(id, nameof(id));

            Logger.LogDebug($"Deleting {TableDetails.Name} with Id {id}");
            await _repository.Delete(id);
            Logger.LogDebug($"Deleted {TableDetails.Name} with id {id}");

            if (TableDetails.LogDelete)
            {
                Logger.LogDebug($"Delete of {TableDetails.Name} with id {id} sent to external Logger");
                await _auditDomain.AuditDelete(id, User.UserId, TableDetails);
            }
        }
    }
}
