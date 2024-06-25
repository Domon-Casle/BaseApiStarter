using BaseDomain;
using BaseDomain.Audit;
using CoreUtilities;

namespace BaseDomainUnitTests.TestDomains
{
    public interface ITestDomain : IBaseDomain<TestEntity, ITestRepository>
    {
    }

    public class TestDomain(
        ITestRepository repo, 
        IUserPrincipal userPrincapal, 
        IBaseLogger logger,
        IAuditDomain auditDomain
    ) : BaseDomain<TestEntity, ITestRepository>(repo, userPrincapal, logger, auditDomain), ITestDomain
    {
    }
}
