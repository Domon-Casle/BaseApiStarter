using BaseDomain;
using BaseDomain.Audit;
using CoreUtilities.Logger;

namespace BaseDomainUnitTests.TestDomains
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
