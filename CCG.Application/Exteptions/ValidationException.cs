namespace CCG.Application.Exteptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message = "Validation failed 400") : base(message){}
    }
}