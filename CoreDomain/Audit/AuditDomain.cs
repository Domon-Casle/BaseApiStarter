using CoreDomain.Audit.Repositories;
using CoreUtilities;
using CoreUtilities.DI;
using CoreUtilities.Logger;

namespace CoreDomain.Audit
{
    public interface IAuditDomain
    {
        List<Variance> AreThereChanges<TEntity>(TEntity? oldEntity, TEntity newEntity, TableCache tableCache) where TEntity : BaseModel;
        Task AuditRead(Guid entityId, TableCache tableCache);
        Task AuditCreate<TEntity>(TEntity entity, TableCache tableCache) where TEntity : BaseModel;
        Task AuditChanges<TEntity>(TEntity entity, List<Variance> changes, TableCache tableCache) where TEntity : BaseModel;
        Task AuditDelete(Guid entityId, TableCache tableCache);
    }

    [InjectDependency(typeof(IAuditDomain))]
    public class AuditDomain(IBaseLogger logger, IUserPrincipal userPrincipal, IAuditRepository auditRepository) : IAuditDomain
    {
        private readonly IBaseLogger _logger = logger;
        private readonly IUserPrincipal _userPrincipal = userPrincipal;
        private readonly IAuditRepository _auditRepository = auditRepository;

        public Task AuditRead(Guid entityId, TableCache tableCache)
        {
            Require.NotEmpty(entityId, nameof(entityId));
            _logger.LogDebug($"Auditing read of {tableCache.Name} id of {entityId} by user id {_userPrincipal.UserId}");
            var audit = new Audit(entityId, tableCache.Name, _userPrincipal.UserId, AuditType.Read);
            return _auditRepository.Create(audit);
        }

        public Task AuditCreate<TEntity>(TEntity entity, TableCache tableCache) where TEntity : BaseModel
        {
#pragma warning disable CS8604 // Possible null reference argument.
            Require.NotNull(entity, nameof(entity));
#pragma warning restore CS8604 // Possible null reference argument.

            _logger.LogDebug($"Auditing creation of {tableCache.Name} id of {entity.Id}");
            var audit = new Audit(entity.Id, tableCache.Name, _userPrincipal.UserId, AuditType.Create);
            return _auditRepository.Create(audit);
        }

        public Task AuditChanges<TEntity>(TEntity entity, List<Variance> changes, TableCache tableCache) where TEntity : BaseModel
        {
#pragma warning disable CS8604 // Possible null reference argument.
            Require.NotNull(entity, nameof(entity));
#pragma warning restore CS8604 // Possible null reference argument.
            Require.NotNullOrEmpty(changes, nameof(changes));

            _logger.LogDebug($"Auditing changes to {tableCache.Name} id of {entity.Id}");
            var audit = new Audit(entity.Id, tableCache.Name, _userPrincipal.UserId, AuditType.Update);
            return _auditRepository.Create(audit);
        }

        public Task AuditDelete(Guid entityId, TableCache tableCache)
        {
            Require.NotEmpty(entityId, nameof(entityId));
            _logger.LogDebug($"Auditing delete of {tableCache.Name} id of {entityId} by user id {_userPrincipal.UserId}");
            var audit = new Audit(entityId, tableCache.Name, _userPrincipal.UserId, AuditType.Delete);
            return _auditRepository.Create(audit);
        }

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
