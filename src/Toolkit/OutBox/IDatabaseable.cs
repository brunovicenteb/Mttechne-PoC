using Mttechne.Toolkit.Interfaces;
using Mttechne.Toolkit.TransactionalOutBox;

namespace Mttechne.Toolkit.OutBox;

public interface IDatabaseable : IOutboxBuilder
{
    public IBrokeable UseDatabase();
    public IBrokeable DoNotUseDatabase();
}