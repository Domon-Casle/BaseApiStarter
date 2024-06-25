using BaseDomain;
using BaseDomain.Audit;
using BaseDomainUnitTests.TestDomains;
using CoreUtilities;
using CoreUtilities.Exceptions;
using Moq;

namespace BaseDomainUnitTests
{
    public class BaseDomainTests
    {
        private readonly Mock<IUserPrincipal> mockUserPrincipal = new(MockBehavior.Strict);
        private readonly Mock<ITestRepository> mockRepository = new(MockBehavior.Strict);
        private readonly Mock<IBaseLogger> mockLogger = new(MockBehavior.Loose);
        private readonly Mock<IAuditDomain> mockAuditDomain = new(MockBehavior.Strict);

        private Mock<TestDomain> GetDomain()
        {
            return new Mock<TestDomain>(
                mockRepository.Object,
                mockUserPrincipal.Object,
                mockLogger.Object,
                mockAuditDomain.Object
            )
            { CallBase = true };
        }

        // TODO Validate if we put attribute on it triggers event

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
                    It.IsAny<TestEntity>(),
                    It.IsAny<TestEntity>(),
                    It.IsAny<TableCache>()))
                .Returns(new List<Variance>());

            // Act
            await domain.Object.Update(entity);

            // Assert
            // Repo update was not called proved worked
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
                .Returns(new List<Variance>());

            // Act
            await domain.Object.Update(entity);

            // Assert
            // Repo was called and setup above proved it worked
        }

        // TODO Validate update only occurs if actual changes

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