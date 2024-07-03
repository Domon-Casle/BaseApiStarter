using CoreDomain;
using CoreDomain.Audit;
using CoreUtilities.Logger;

namespace CoreDomainUnitTests.TestDomains
{
    public interface ITestDomain : IBaseDomain<TestEntity, ITestBaseRepository>
    {
    }

    public class TestBaseDomain(
        ITestBaseRepository repo, 
        IUserPrincipal userPrincapal, 
        IBaseLogger logger,
        IAuditDomain auditDomain
    ) : BaseDomain<TestEntity, ITestBaseRepository>(repo, userPrincapal, logger, auditDomain), ITestDomain
    {
    }
}
