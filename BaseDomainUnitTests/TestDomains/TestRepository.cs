using BaseDomain;

namespace BaseDomainUnitTests.TestDomains
{
    public interface ITestRepository : IBaseRepository<TestEntity>
    {
    }

    public class TestRepository : BaseRepository<TestEntity>, ITestRepository
    {
    }
}
