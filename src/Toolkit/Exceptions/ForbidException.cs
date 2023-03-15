namespace Mttechne.Toolkit.Exceptions;

public sealed class ForbidException : BaseException
{
    public ForbidException()
        : base()
    {
    }
    public ForbidException(string message)
        : base(message)
    {
    }
    public ForbidException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}