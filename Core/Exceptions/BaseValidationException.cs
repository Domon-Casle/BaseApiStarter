namespace CoreUtilities.Exceptions
{
    public class BaseValidationException : Exception
    {
        public List<string> ValidationErrors = [];

        public BaseValidationException(string validationError)
        {
            ValidationErrors.Add(validationError);
        }

        public BaseValidationException(List<string> validationErrors)
        {
            validationErrors.AddRange(ValidationErrors);
        }

        public void AddValidationError(string validationError)
        {
            ValidationErrors.Add(validationError);
        }
    }
}
