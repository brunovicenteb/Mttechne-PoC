namespace Mttechne.Toolkit.Exceptions;

public sealed class DomainRuleException : BaseException
{
    public DomainRuleException()
        : base()
    {
    }
    public DomainRuleException(string mensagem)
        : base(mensagem)
    {
    }
    public DomainRuleException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}