namespace Mttechne.Toolkit;

public abstract class DisposableObject : IDisposable
{
    ~DisposableObject()
    {
        Dispose(false);
    }

    private bool _Disposed = false;

    protected abstract void DoDispose();

    private void Dispose(bool disposing)
    {
        if (_Disposed)
            return;
        _Disposed = true;
        if (disposing)
            return;
        DoDispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}