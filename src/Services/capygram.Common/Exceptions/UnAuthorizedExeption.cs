namespace capygram.Common.Exceptions
{
    public class UnAuthorizedExeption : DomainException
    {
        public UnAuthorizedExeption()
        {
        }

        public UnAuthorizedExeption(string? message) : base(message)
        {
        }
    }
}
