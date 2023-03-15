using Microsoft.AspNetCore.Builder;

namespace Mttechne.Toolkit.Interfaces;

public interface IOutboxBuilder
{
    public WebApplicationBuilder Builder { get; }
}