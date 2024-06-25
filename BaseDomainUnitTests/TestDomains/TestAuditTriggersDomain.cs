using BaseDomain;
using BaseDomain.Audit;
using CoreUtilities.Logger;

namespace BaseDomainUnitTests.TestDomains
{
    public interface ITestAuditTriggersDomain : IBaseDomain<TestAuditTriggersEntity, ITestAuditRepository>
    {
    }

    public class TestAuditTriggersDomain(
        ITestAuditRepository repo, 
        IUserPrincipal userPrincapal, 
        IBaseLogger logger,
        IAuditDomain auditDomain
    ) : BaseDomain<TestAuditTriggersEntity, ITestAuditRepository>(repo, userPrincapal, logger, auditDomain), ITestAuditTriggersDomain
    {
    }
}
