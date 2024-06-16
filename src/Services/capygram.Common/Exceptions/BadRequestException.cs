
using System.Runtime.Serialization;

namespace capygram.Common.Exceptions
{
    public class BadRequestException : DomainException
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string? message) : base(message)
        {

        }

    }
}
