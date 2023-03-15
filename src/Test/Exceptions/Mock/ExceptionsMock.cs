using System.Collections;
using Mttechne.Toolkit.Exceptions;

namespace Mttechne.Test.Exceptions.Mock;
public class ExceptionsMock : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { new NotFoundException() };
        yield return new object[] { new NotFoundException("NotFoundException", new NotFoundException("NotFoundException")) };
        yield return new object[] { new ForbidException() };
        yield return new object[] { new ForbidException("ForbidException", new ForbidException("ForbidException")) };
        yield return new object[] { new UnauthorizedException() };
        yield return new object[] { new UnauthorizedException("UnauthorizedException", new UnauthorizedException("UnauthorizedException")) };
        yield return new object[] { new BadRequestException() };
        yield return new object[] { new BadRequestException("BadRequestException", new BadRequestException("BadRequestException")) };
        yield return new object[] { new DomainRuleException() };
        yield return new object[] { new DomainRuleException("DomainRuleException", new DomainRuleException("DomainRuleException")) };
        yield return new object[] { new DuplicateRegistrationException() };
        yield return new object[] { new DuplicateRegistrationException("DuplicateRegistrationException", 
            new DuplicateRegistrationException("DuplicateRegistrationException")) };
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}