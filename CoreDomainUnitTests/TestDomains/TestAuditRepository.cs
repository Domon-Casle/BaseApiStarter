using CoreDomain;

namespace CoreDomainUnitTests.TestDomains
{
    public interface ITestAuditRepository : IBaseRepository<TestAuditTriggersEntity>
    {
    }

    public class TestAuditRepository : BaseRepository<TestAuditTriggersEntity>, ITestAuditRepository
    {
    }
}
