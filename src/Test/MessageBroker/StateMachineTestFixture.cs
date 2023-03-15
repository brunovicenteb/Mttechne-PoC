using MassTransit;
using MassTransit.Testing;
using Mttechne.Toolkit.TransactionalOutBox;
using Microsoft.Extensions.DependencyInjection;

namespace Mttechne.Test.MessageBroker;

public class StateMachineTestFixtureFixture<TDbContext> : StarterIoC<TDbContext>
    where TDbContext : OutBoxDbContext, new()
{
    public StateMachineTestFixtureFixture()
    {
        TestHarness = Provider.GetRequiredService<ITestHarness>();
        TestHarness.Start();
    }

    internal ITestHarness TestHarness;

    protected override void DoDispose()
    {
        TestHarness?.Stop();
        base.DoDispose();
    }
}

public class StateMachineTestFixture<TDbContext, TStateMachine, TInstance> : IClassFixture<StateMachineTestFixtureFixture<TDbContext>>
    where TDbContext : OutBoxDbContext, new()
    where TStateMachine : class, SagaStateMachine<TInstance>
    where TInstance : class, SagaStateMachineInstance
{
    protected readonly TStateMachine Machine;
    protected readonly ServiceProvider Provider;
    protected readonly ISagaStateMachineTestHarness<TStateMachine, TInstance> SagaHarness;
    protected readonly ITestHarness TestHarness;

    public StateMachineTestFixture(StateMachineTestFixtureFixture<TDbContext> fixture)
    {
        Provider = fixture.Provider;
        TestHarness = Provider.GetRequiredService<ITestHarness>();
        SagaHarness = TestHarness.GetSagaStateMachineHarness<TStateMachine, TInstance>();
        Machine = SagaHarness.StateMachine;
    }
}