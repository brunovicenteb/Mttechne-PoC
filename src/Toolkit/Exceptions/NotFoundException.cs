namespace Mttechne.Toolkit.Exceptions;

public sealed class NotFoundException : BaseException
{
    public NotFoundException()
        : base()
    {
    }
    public NotFoundException(string message)
        : base(message)
    {
    }
    public NotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}