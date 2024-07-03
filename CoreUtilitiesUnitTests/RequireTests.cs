using CoreUtilities;
using CoreUtilities.Exceptions;

namespace CoreUtilitiesUnitTests
{
    public class RequireTests
    {
        [Fact]
        public void NotNullOrEmpty_Success()
        {
            // arrange

            // Act
            Require.NotNullOrEmpty("Test", "NotNullOrEmpty");

            //Assert
        }

        [Fact]
        public void NotNullOrEmpty_FailureNull()
        {
            // arrange

            // Act
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            Action act = () => Require.NotNullOrEmpty((string)null, "NotNullOrEmpty");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            //Assert
            Assert.Throws<BaseValidationException>(act);
        }

        [Fact]
        public void NotNullOrEmpty_FailureEmpty()
        {
            // arrange

            // Act
            Action act = () => Require.NotNullOrEmpty(string.Empty, "NotNullOrEmpty");

            //Assert
            Assert.Throws<BaseValidationException>(act);
        }

        [Fact]
        public void NotEmpty_Success()
        {
            // arrange

            // Act
            Require.NotEmpty(Guid.NewGuid(), "NotEmpty");

            //Assert
        }

        [Fact]
        public void NotEmpty_Failure()
        {
            // arrange

            // Act
            Action act = () => Require.NotEmpty(Guid.Empty, "NotEmpty");

            //Assert
            Assert.Throws<BaseValidationException>(act);
        }

        [Fact]
        public void NotNull_Success()
        {
            // arrange

            // Act
            Require.NotNull(Guid.NewGuid(), "NotNull");

            //Assert
        }

        [Fact]
        public void NotNull_Failure()
        {
            // arrange

            // Act
            Action act = () => Require.NotNull(null, "NotNull");

            //Assert
            Assert.Throws<BaseValidationException>(act);
        }

        [Fact]
        public void NotNullOrEmptyList_Success()
        {
            // arrange

            // Act
            Require.NotNullOrEmpty(new List<Object>() { Guid.NewGuid() }, "NotNullOrEmptyList");

            //Assert
        }

        [Fact]
        public void NotNullOrEmptyList_FailureNull()
        {
            // arrange

            // Act
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Action act = () => Require.NotNullOrEmpty((List<Object>)null, "NotNullOrEmptyList");
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            //Assert
            Assert.Throws<BaseValidationException>(act);
        }

        [Fact]
        public void NotNullOrEmptyList_FailureEmpty()
        {
            // arrange

            // Act
            Action act = () => Require.NotNullOrEmpty(new List<Object>() { }, "NotNullOrEmptyList");

            //Assert
            Assert.Throws<BaseValidationException>(act);
        }
    }
}