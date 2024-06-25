using CoreUtilities;

namespace BaseDomain.Audit
{
    public interface IAuditDomain
    {
        List<Variance> AreThereChanges<TEntity>(TEntity? oldEntity, TEntity newEntity, TableCache tableCache) where TEntity : BaseModel;
        Task AuditRead(Guid entityId, Guid userId, TableCache tableCache);
        Task AuditCreate<TEntity>(TEntity entity, TableCache tableCache) where TEntity : BaseModel;
        Task AuditChanges<TEntity>(TEntity entity, List<Variance> changes, TableCache tableCache) where TEntity : BaseModel;
        Task AuditDelete(Guid entityId, Guid userId, TableCache tableCache);
    }

    public class AuditDomain(IBaseLogger logger) : IAuditDomain
    {
        private readonly IBaseLogger _logger = logger;

        public Task AuditRead(Guid entityId, Guid userId, TableCache tableCache)
        {
            Require.NotEmpty(entityId, nameof(entityId));
            Require.NotEmpty(userId, nameof(userId));
            _logger.LogDebug($"Auditing read of {tableCache.Name} id of {entityId} by user id {userId}");
            return Task.CompletedTask;
        }

        public Task AuditCreate<TEntity>(TEntity entity, TableCache tableCache) where TEntity : BaseModel
        {
#pragma warning disable CS8604 // Possible null reference argument.
            Require.NotNull(entity, nameof(entity));
#pragma warning restore CS8604 // Possible null reference argument.

            _logger.LogDebug($"Auditing creation of {tableCache.Name} id of {entity.Id}");
            return Task.CompletedTask;
        }

        public Task AuditChanges<TEntity>(TEntity entity, List<Variance> changes, TableCache tableCache) where TEntity : BaseModel
        {
#pragma warning disable CS8604 // Possible null reference argument.
            Require.NotNull(entity, nameof(entity));
#pragma warning restore CS8604 // Possible null reference argument.
            Require.NotNullOrEmpty(changes, nameof(changes));

            _logger.LogDebug($"Auditing changes to {tableCache.Name} id of {entity.Id}");
            return Task.CompletedTask;
        }

        public Task AuditDelete(Guid entityId, Guid userId, TableCache tableCache)
        {
            Require.NotEmpty(entityId, nameof(entityId));
            Require.NotEmpty(userId, nameof(userId));
            _logger.LogDebug($"Auditing delete of {tableCache.Name} id of {entityId} by user id {userId}");
            return Task.CompletedTask;
        }

        // TODO Unit test
        public List<Variance> AreThereChanges<TEntity>(TEntity? oldEntity, TEntity newEntity, TableCache tableCache) where TEntity : BaseModel
        {
            var variances = new List<Variance>();
            if (oldEntity == null && newEntity == null)
            {
                return variances;
            }

            foreach (var field in tableCache.Fields)
            {
                var oldValue = field.GetValue(oldEntity);
                var newValue = field.GetValue(newEntity);
                if (!Equals(oldValue, newValue))
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    variances.Add(
                        new Variance(
                            field.Name,
                            oldValue != null ? oldValue.ToString() : string.Empty,
                            newValue != null ? newValue.ToString() : string.Empty
                        )
                    );
#pragma warning restore CS8604 // Possible null reference argument.
                }
            }

            return variances;
        }
    }
}
