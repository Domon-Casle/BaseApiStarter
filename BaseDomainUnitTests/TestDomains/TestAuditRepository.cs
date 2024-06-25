using BaseDomain;

namespace BaseDomainUnitTests.TestDomains
{
    public interface ITestAuditRepository : IBaseRepository<TestAuditTriggersEntity>
    {
    }

    public class TestAuditRepository : BaseRepository<TestAuditTriggersEntity>, ITestAuditRepository
    {
    }
}
