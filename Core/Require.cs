using CoreUtilities.Exceptions;

namespace CoreUtilities
{
    public static class Require
    {
        public static void NotNullOrEmpty(string message, string variableName)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new BaseValidationException(variableName);
            }
        }

        public static void NotEmpty(Guid guid, string variableName)
        {
            if (guid.Equals(Guid.Empty))
            {
                throw new BaseValidationException(variableName);
            }
        }

        public static void NotNull(object value, string variableName)
        {
            if (value == null)
            {
                throw new BaseValidationException(variableName);
            }
        }

        public static void NotNullOrEmpty(IEnumerable<object> value, string variableName)
        {
            if (value == null || value.Count() == 0)
            {
                throw new BaseValidationException(variableName);
            }
        }
    }
}
