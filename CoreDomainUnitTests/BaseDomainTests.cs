using CoreDomain;
using CoreDomain.Audit;
using CoreDomainUnitTests.TestDomains;
using CoreUtilities.Exceptions;
using CoreUtilities.Logger;
using Moq;

namespace CoreDomainUnitTests
{
    public class BaseDomainTests
    {
        private readonly Mock<IUserPrincipal> mockUserPrincipal = new(MockBehavior.Strict);
        private readonly Mock<ITestBaseRepository> mockRepository = new(MockBehavior.Strict);
        private readonly Mock<ITestAuditRepository> mockAuditRepository = new(MockBehavior.Strict);
        private readonly Mock<IBaseLogger> mockLogger = new(MockBehavior.Loose);
        private readonly Mock<IAuditDomain> mockAuditDomain = new(MockBehavior.Strict);

        private Mock<TestBaseDomain> GetDomain()
        {
            return new Mock<TestBaseDomain>(
                mockRepository.Object,
                mockUserPrincipal.Object,
                mockLogger.Object,
                mockAuditDomain.Object
            )
            { CallBase = true };
        }

        private Mock<TestAuditTriggersDomain> GetAuditDomain()
        {
            return new Mock<TestAuditTriggersDomain>(
                mockAuditRepository.Object,
                mockUserPrincipal.Object,
                mockLogger.Object,
                mockAuditDomain.Object
            )
            { CallBase = true };
        }

        [Fact]
        public async Task Get_SuccessAudit()
        {
            // Arrange
            var expected = new TestAuditTriggersEntity();
            var domain = GetAuditDomain();
            Guid entityId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            mockAuditRepository.Setup(r => r.Get(entityId)).ReturnsAsync(expected);
            mockUserPrincipal.Setup(x => x.UserId).Returns(userId);
            mockAuditDomain.Setup(r => r.AuditRead(entityId, userId, It.IsAny<TableCache>())).Returns(Task.CompletedTask);

            // Act
            var result = await domain.Object.Get(entityId);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Get_Success()
        {
            // Arrange
            var expected = new TestEntity();
            var domain = GetDomain();
            mockRepository.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(expected);

            // Act
            var result = await domain.Object.Get(Guid.NewGuid());

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Get_FailureIdEmpty()
        {
            // Arrange
            var expected = new TestEntity();
            var domain = GetDomain();
            mockRepository.Setup(r => r.Get(It.IsAny<Guid>())).ReturnsAsync(expected);

            // Act
            Func<Task> act = () => domain.Object.Get(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<BaseValidationException>(() => act());
        }

        [Fact]
        public async Task Get_FailureEntityNull()
        {
            // Arrange
            var domain = GetDomain();
            mockRepository.Setup(r => r.Get(It.IsAny<Guid>())).Returns(Task.FromResult<TestEntity?>(null));

            // Act
            Func<Task> act = () => domain.Object.Get(Guid.NewGuid());

            // Assert
            await Assert.ThrowsAsync<BaseValidationException>(() => act());
        }

        [Fact]
        public async void Create_Success()
        {
            // Arrange
            var domain = GetDomain();
            Guid userId = Guid.NewGuid();
            Guid newEntityGuid = Guid.NewGuid();
            mockUserPrincipal.Setup(x => x.UserId).Returns(userId);
            var entity = new TestEntity();
            mockRepository.Setup(x => x.Create(entity)).ReturnsAsync(newEntityGuid);

            // Act
            var newGuid = await domain.Object.Create(entity);

            // Assert
            Assert.Equal(newEntityGuid, newGuid);
        }

        [Fact]
        public async void Create_SuccessAudit()
        {
            // Arrange
            var domain = GetAuditDomain();
            Guid userId = Guid.NewGuid();
            Guid newEntityGuid = Guid.NewGuid();
            mockUserPrincipal.Setup(x => x.UserId).Returns(userId);
            var entity = new TestAuditTriggersEntity();
            mockAuditRepository.Setup(x => x.Create(entity)).ReturnsAsync(newEntityGuid);
            mockAuditDomain.Setup(x => x.AuditCreate(entity, It.IsAny<TableCache>())).Returns(Task.CompletedTask);

            // Act
            var newGuid = await domain.Object.Create(entity);

            // Assert
            Assert.Equal(newEntityGuid, newGuid);
        }

        [Fact]
        public async void Create_NullEntity()
        {
            // Arrange
            var domain = GetDomain();
            Guid userId = Guid.NewGuid();
            mockUserPrincipal.Setup(x => x.UserId).Returns(userId);

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Func<Task> act = () => domain.Object.Create(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Assert
            await Assert.ThrowsAsync<BaseValidationException>(() => act());
        }

        [Fact]
        public async void Update_SuccessNoChanges()
        {
            // Arrange
            var domain = GetDomain();
            Guid userId = Guid.NewGuid();
            Guid newEntityGuid = Guid.NewGuid();
            mockUserPrincipal.Setup(x => x.UserId).Returns(userId);
            var entity = new TestEntity();
            domain.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(entity);
            mockAuditDomain.Setup(x =>
                x.AreThereChanges(
                    entity,
                    entity,
                    It.IsAny<TableCache>()))
                .Returns(new List<Variance>());

            // Act
            await domain.Object.Update(entity);

            // Assert
        }

        [Fact]
        public async void Update_SuccessChanges()
        {
            // Arrange
            var domain = GetDomain();
            Guid userId = Guid.NewGuid();
            Guid newEntityGuid = Guid.NewGuid();
            mockUserPrincipal.Setup(x => x.UserId).Returns(userId);
            var entity = new TestEntity();
            mockRepository.Setup(x => x.Update(entity)).Returns(Task.CompletedTask);
            domain.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(entity);
            mockAuditDomain.Setup(x =>
                x.AreThereChanges(
                    It.IsAny<TestEntity>(),
                    It.IsAny<TestEntity>(),
                    It.IsAny<TableCache>()))
                .Returns(new List<Variance>()
                {
                    new Variance("Test", "Old", "New")
                });

            // Act
            await domain.Object.Update(entity);

            // Assert
            // Repo was called and setup above proved it worked
        }

        [Fact]
        public async void Update_SuccessChangesAudit()
        {
            // Arrange
            var domain = GetAuditDomain();
            Guid userId = Guid.NewGuid();
            Guid newEntityGuid = Guid.NewGuid();
            mockUserPrincipal.Setup(x => x.UserId).Returns(userId);
            var entity = new TestAuditTriggersEntity();
            var variances = new List<Variance> { new Variance("Test", "Old", "New") };
            mockAuditRepository.Setup(x => x.Update(entity)).Returns(Task.CompletedTask);
            domain.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(entity);
            mockAuditDomain.Setup(x =>
                x.AreThereChanges(
                    It.IsAny<TestAuditTriggersEntity>(),
                    It.IsAny<TestAuditTriggersEntity>(),
                    It.IsAny<TableCache>()))
                .Returns(variances);
            mockAuditDomain.Setup(x => x.AuditChanges(It.IsAny<TestAuditTriggersEntity>(), userId, variances, It.IsAny<TableCache>())).Returns(Task.CompletedTask);

            // Act
            await domain.Object.Update(entity);

            // Assert
        }

        [Fact]
        public async void Update_NullEntity()
        {
            // Arrange
            var domain = GetDomain();
            Guid userId = Guid.NewGuid();
            mockUserPrincipal.Setup(x => x.UserId).Returns(userId);

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Func<Task> act = () => domain.Object.Update(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Assert
            await Assert.ThrowsAsync<BaseValidationException>(() => act());
        }

        [Fact]
        public async void Delete_Success()
        {
            // Arrange
            var domain = GetDomain();
            Guid userId = Guid.NewGuid();
            Guid entityGuid = Guid.NewGuid();
            mockRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            await domain.Object.Delete(entityGuid);

            // Assert            
        }

        [Fact]
        public async void Delete_SuccessAudit()
        {
            // Arrange
            var domain = GetAuditDomain();
            Guid userId = Guid.NewGuid();
            Guid entityGuid = Guid.NewGuid();
            mockUserPrincipal.Setup(x => x.UserId).Returns(userId);
            mockAuditRepository.Setup(x => x.Delete(entityGuid)).Returns(Task.CompletedTask);
            mockAuditDomain.Setup(x => x.AuditDelete(entityGuid, userId, It.IsAny<TableCache>())).Returns(Task.CompletedTask);

            // Act
            await domain.Object.Delete(entityGuid);

            // Assert            
        }

        [Fact]
        public async void Delete_NullEntity()
        {
            // Arrange
            var domain = GetDomain();
            Guid userId = Guid.NewGuid();
            mockUserPrincipal.Setup(x => x.UserId).Returns(userId);

            // Act
            Func<Task> act = () => domain.Object.Delete(Guid.Empty);

            // Assert
            await Assert.ThrowsAsync<BaseValidationException>(() => act());
        }
    }
}