using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Mttechne.Toolkit;

namespace Mttechne.Test;

public class ResilienceTest
{
    #region resilienceTestException
    private class resilienceTestException : Exception
    {
        public resilienceTestException(string message)
            : base(message)
        {
        }
    }
    #endregion

    #region resilienceLogger
    private class resilienceLogger : ILogger, IDisposable
    {
        private Exception _Exception;
        public Exception Exception => _Exception;

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _Exception = exception;
        }
    }
    #endregion

    [Fact]
    public async void TryExecuteSuccessfullyOnFirstAttemp()
    {
        int count = 0;
        var result = await Resilience.TryExecute(async () =>
        {
            count++;
            await Task.CompletedTask;
            return nameof(TryExecuteSuccessfullyOnFirstAttemp);
        });
        Assert.Equal(1, count);
        Assert.Equal(nameof(TryExecuteSuccessfullyOnFirstAttemp), result);
    }

    [Fact]
    public async void TryExecuteSuccessfullyOnSecondAttemp()
    {
        int count = 0;
        var result = await Resilience.TryExecute(async () =>
        {
            count++;
            if (count == 1)
                throw new resilienceTestException(nameof(TryExecuteSuccessfullyOnSecondAttemp));
            await Task.CompletedTask;
            return nameof(TryExecuteSuccessfullyOnSecondAttemp);
        });
        Assert.Equal(2, count);
        Assert.Equal(nameof(TryExecuteSuccessfullyOnSecondAttemp), result);
    }


    [Fact]
    public async void TryExecuteFailWithLogger()
    {
        var myLogger = new resilienceLogger();
        var result = await Resilience.TryExecute<string>(() =>
        {
            throw new resilienceTestException(nameof(TryExecuteFailWithLogger));
        }, logger: myLogger, failMessage: nameof(TryExecuteFailWithLogger));
        Assert.Null(result);
        Assert.NotNull(myLogger.Exception);
        Assert.IsType<resilienceTestException>(myLogger.Exception);
        Assert.Equal(nameof(TryExecuteFailWithLogger), myLogger.Exception.Message);
    }

    [Fact]
    public async void TryExecuteFailWithLoggerButWithoutFailMessage()
    {
        var myLogger = new resilienceLogger();
        var result = await Resilience.TryExecute<string>(() =>
        {
            throw new resilienceTestException(nameof(TryExecuteFailWithLoggerButWithoutFailMessage));
        }, logger: myLogger);
        Assert.Null(result);
        Assert.Null(myLogger.Exception);
    }

    [Fact]
    public async void TryExecuteFailWithFailMessageButWithoutLogger()
    {
        var result = await Resilience.TryExecute<string>(() =>
        {
            throw new resilienceTestException(nameof(TryExecuteFailWithFailMessageButWithoutLogger));
        }, failMessage: nameof(TryExecuteFailWithFailMessageButWithoutLogger));
        Assert.Null(result);
    }

    [Fact]
    public async void TryExecuteFailDefaultParameters()
    {
        int count = 0;
        var result = await Resilience.TryExecute<string>(() =>
        {
            count++;
            throw new resilienceTestException(nameof(TryExecuteFailDefaultParameters));
        });
        Assert.Null(result);
        Assert.Equal(6, count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async void TryExecuteFailDifferentsRetrails(int retryCount)
    {
        int count = 0;
        var result = await Resilience.TryExecute<string>(() =>
        {
            count++;
            throw new resilienceTestException(nameof(TryExecuteFailDifferentsRetrails));
        }, retryCount: retryCount);
        Assert.Null(result);
        Assert.Equal(retryCount + 1, count);
    }
}