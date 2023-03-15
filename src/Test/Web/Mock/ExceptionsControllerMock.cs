using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Mttechne.Toolkit.Exceptions;

namespace Mttechne.Test.Web.Mock
{
    public class ExceptionsControllerMock : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new NotFoundException(), HttpStatusCode.NotFound };
            yield return new object[] { new ForbidException(), HttpStatusCode.Forbidden };
            yield return new object[] { new UnauthorizedException(), HttpStatusCode.Unauthorized };
            yield return new object[] { new BadRequestException(), HttpStatusCode.BadRequest };
            yield return new object[] { new DomainRuleException(), HttpStatusCode.BadRequest };
            yield return new object[] { new DuplicateRegistrationException(), HttpStatusCode.InternalServerError };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
