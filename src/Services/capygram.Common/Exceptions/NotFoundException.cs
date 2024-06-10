namespace capygram.Common.Exceptions
{
    public class NotFoundException : DomainException
    {
        public NotFoundException() { }
        public NotFoundException(string? message) : base(message)
        {
        }
    }
}
