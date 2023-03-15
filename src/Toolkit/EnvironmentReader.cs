namespace Mttechne.Toolkit;

public static class EnvironmentReader
{
    public static T Read<T>(string varName, T defaultValue = default, string varEmptyError = null)
        where T : IComparable, IConvertible
    {
        if (varName.IsEmpty())
            throw new ArgumentNullException(nameof(varName));
        bool hasDefault = !EqualityComparer<T>.Default.Equals(defaultValue, default);
        var value = Environment.GetEnvironmentVariable(varName);
        if (value.IsEmpty())
            if (hasDefault)
                return defaultValue;
            else
                throw new NullReferenceException(varEmptyError);
        var type = typeof(T);
        if (type.IsEnum)
        {
            if (Enum.TryParse(type, value.ToString(), out object ret))
                return (T)ret;
            return defaultValue;
        }
        T result = (T)Convert.ChangeType(value, typeof(T));
        if (!EqualityComparer<T>.Default.Equals(result, defaultValue))
            return result;
        return defaultValue;
    }
}