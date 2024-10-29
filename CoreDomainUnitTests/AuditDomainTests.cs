using CoreDomain;
using CoreDomain.Audit;
using CoreDomain.Audit.Repositories;
using CoreDomainUnitTests.TestDomains;
using CoreUtilities.Logger;
using Moq;

namespace CoreDomainUnitTests
{
    public class AuditDomainTests
    {
        private readonly Mock<IBaseLogger> mockLogger = new(MockBehavior.Loose);
        private readonly Mock<IUserPrincipal> mockUserPrincipal = new(MockBehavior.Loose);
        private readonly Mock<IAuditRepository> mockAuditRepository = new(MockBehavior.Loose);

        private Mock<AuditDomain> GetDomain()
        {
            return new Mock<AuditDomain>(
                mockLogger.Object,
                mockUserPrincipal.Object,
                mockAuditRepository.Object
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
