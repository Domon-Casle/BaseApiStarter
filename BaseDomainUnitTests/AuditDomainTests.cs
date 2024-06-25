using BaseDomain.Audit;
using CoreUtilities.Logger;
using Moq;

namespace BaseDomainUnitTests
{
    public class AuditDomainTests
    {
        private readonly Mock<IBaseLogger> mockLogger = new(MockBehavior.Loose);

        private Mock<AuditDomain> GetDomain()
        {
            return new Mock<AuditDomain>(
                mockLogger.Object
            )
            { CallBase = true };
        }

        [Fact]
        public async Task AuditCreate_Success()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
