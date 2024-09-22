namespace CCG.Application.Exteptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized 401") : base(message){}
    }
}