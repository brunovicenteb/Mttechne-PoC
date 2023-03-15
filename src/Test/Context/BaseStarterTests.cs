using Microsoft.Extensions.DependencyInjection;

namespace Mttechne.Test.Context;

public abstract class BaseStarterTests : IDisposable
{
    protected ServiceProvider Provider;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private async void Dispose(bool isDisposing)
    {
        if (!isDisposing)
            return;
        if (Provider != null)
            await Provider.DisposeAsync();
    }

    protected static void ClearVars(string[] varNames)
    {
        foreach (var varName in varNames)
            Environment.SetEnvironmentVariable(varName, string.Empty);
    }

    protected static string[] SetVars(params string[] varNamesAndValues)
    {
        var varNames = new List<string>();
        int n = varNamesAndValues == null ? 0 : varNamesAndValues.Length;
        for (int i = 0; i < n; i += 2)
        {
            var varName = varNamesAndValues[i];
            var varValue = varNamesAndValues[i + 1];
            varNames.Add(varName);
            Environment.SetEnvironmentVariable(varName, varValue);
        }
        return varNames.ToArray();
    }
}