namespace CCG.Application.Exteptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Not Found 404") : base(message) {}
    }
}