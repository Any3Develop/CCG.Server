namespace CCG.Application.Exteptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "Forbidden 403") : base(message){}
    }
}