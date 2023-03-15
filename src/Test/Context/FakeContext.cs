using Microsoft.EntityFrameworkCore;
using Mttechne.Toolkit.OutBox.Producer;
using Mttechne.Toolkit.TransactionalOutBox;
using MassTransit.EntityFrameworkCoreIntegration;

namespace Mttechne.Test.Context;

public class FakeContext : OutBoxDbContext
{
    public FakeContext() : base(new DbContextOptions<FakeContext>())
    {
    }

    public FakeContext(DbContextOptions<FakeContext> options)
        : base(options)
    {
    }
    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { return Array.Empty<ISagaClassMap>(); }
    }

    protected override SagaAdaptorService CreateSagaAdapter(IServiceProvider serviceProvider, string host)
        => null;
}

