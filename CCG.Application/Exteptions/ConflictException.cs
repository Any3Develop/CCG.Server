namespace CCG.Application.Exteptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message = "Conflict 409") : base(message){}
    }
}