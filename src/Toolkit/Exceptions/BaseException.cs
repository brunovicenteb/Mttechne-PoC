namespace Mttechne.Toolkit.Exceptions;

public abstract class BaseException : Exception
{
    protected BaseException()
        : base()
    {
    }
    protected BaseException(string message)
        : base(message)
    {
    }
    protected BaseException(string message, Exception innerException)
    : base(message, innerException)
    {
    }
}