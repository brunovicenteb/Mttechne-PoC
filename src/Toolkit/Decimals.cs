using System.Text;
using System.Globalization;

namespace Mttechne.Toolkit;

public static class Decimals
{
    public static string ToEnglishString(this decimal value)
        => $"$ {value:0.00}";
}