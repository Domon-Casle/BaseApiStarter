using BaseDomain;
using BaseDomain.Attributes;

namespace BaseDomainUnitTests.TestDomains
{
    [LogCreate]
    [LogUpdate]
    [LogDelete]
    [LogRead]
    public class TestAuditTriggersEntity : BaseModel
    {
    }
}
