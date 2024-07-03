using CoreDomain;

namespace CoreDomainUnitTests.TestDomains
{
    public interface ITestBaseRepository : IBaseRepository<TestEntity>
    {
    }

    public class TestBaseRepository : BaseRepository<TestEntity>, ITestBaseRepository
    {
    }
}
