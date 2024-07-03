using CoreDomain;
using CoreDomain.Attributes;

namespace CoreDomainUnitTests.TestDomains
{
    [LogCreate]
    [LogUpdate]
    [LogDelete]
    [LogRead]
    public class TestAuditTriggersEntity : BaseModel
    {
    }
}
