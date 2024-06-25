using BaseDomain;
using BaseDomain.Audit;
using BaseDomainUnitTests.TestDomains;
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
        public void AuditCreate_Success()
        {
            // Arrange
            var domain = GetDomain();
            var entity = new TestEntity();
            var difEntity = new TestEntity()
            {
                Tester = "Old value"
            };
            var tableCache = new TableCache(typeof(TestEntity));

            // Act
            var variance = domain.Object.AreThereChanges(difEntity, entity, tableCache);

            // Assert
            Assert.True(variance.Any());
        }
    }
}
