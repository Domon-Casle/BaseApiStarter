using CoreUtilities.DI;

namespace CoreDomain.Audit.Repositories
{
    public interface IAuditRepository : IBaseRepository<Audit>
    {
    }

    [InjectDependency(typeof(IAuditRepository))]
    public class AuditRepository : BaseRepository<Audit>, IAuditRepository
    {
        public override Task<Audit?> Get(Guid Id)
        {
            throw new NotImplementedException();
        }

        public override Task Update(Audit entity)
        {
            throw new NotImplementedException();
        }

        public override Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
