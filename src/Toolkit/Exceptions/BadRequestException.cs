namespace Mttechne.Toolkit.Exceptions;

public sealed class BadRequestException : BaseException
{
    public BadRequestException()
        : base()
    {
    }
    public BadRequestException(string pMessage)
        : base(pMessage)
    {
    }
    public BadRequestException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}