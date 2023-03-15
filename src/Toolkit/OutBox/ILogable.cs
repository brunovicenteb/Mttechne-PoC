using Mttechne.Toolkit.Interfaces;

namespace Mttechne.Toolkit.OutBox;

public interface ILogable: IOutboxBuilder
{
    public ITelemetreable UseSerilog();
}