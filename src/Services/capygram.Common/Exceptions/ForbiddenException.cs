namespace capygram.Common.Exceptions
{
    public class ForbiddenException : DomainException
    {
        public ForbiddenException()
        {
        }

        public ForbiddenException(string? message) : base(message)
        {
        }
    }
}
