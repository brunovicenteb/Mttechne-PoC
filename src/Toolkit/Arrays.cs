namespace Mttechne.Toolkit;
public static class Arrays
{
    public static int SafeLength(this Array array)
        => array == null ? 0 : array.Length;
}