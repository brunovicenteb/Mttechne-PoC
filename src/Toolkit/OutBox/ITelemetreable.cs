using Mttechne.Toolkit.Interfaces;

namespace Mttechne.Toolkit.OutBox;

public interface ITelemetreable : IOutboxBuilder
{
    public IDatabaseable UseTelemetry(string telemetryHostVariableName = "TELEMETRY_HOST");

    public IDatabaseable DoNotUseTelemetry();
}