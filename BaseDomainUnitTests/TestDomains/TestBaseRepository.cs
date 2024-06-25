using BaseDomain;

namespace BaseDomainUnitTests.TestDomains
{
    public interface ITestBaseRepository : IBaseRepository<TestEntity>
    {
    }

    public class TestBaseRepository : BaseRepository<TestEntity>, ITestBaseRepository
    {
    }
}
