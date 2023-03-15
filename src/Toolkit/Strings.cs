using System.Text;

namespace Mttechne.Toolkit;

public static class Strings
{
    public static bool IsEmpty(this string value) => string.IsNullOrEmpty(value);

    public static bool IsFilled(this string value) => !value.IsEmpty();

    public static string SafeToLower(this string value) => value.IsEmpty() ? string.Empty : value.ToLower();
    
    public static string SafeToUpper(this string value) => value.IsEmpty() ? string.Empty : value.ToUpper();

    public static string Base64Encode(this string value) => value.IsEmpty() ? string.Empty : Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    
    public static string Base64Decode(this string value) => value.IsEmpty() ? string.Empty : Encoding.UTF8.GetString(Convert.FromBase64String(value));

    public static bool IsValidCPF(this string cpf)
    {
        if (cpf.IsEmpty())
            return false;
        int[] multOne = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiTwo = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf;
        string digito;
        int soma;
        int resto;
        cpf = cpf.Trim();
        cpf = cpf.Replace(".", "").Replace("-", "");
        if (cpf.Length != 11)
            return false;
        tempCpf = cpf.Substring(0, 9);
        soma = 0;
        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multOne[i];
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiTwo[i];
        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito = digito + resto.ToString();
        return cpf.EndsWith(digito);
    }

    public static string Limit(this string value, int limit)
    {
        if (value is null) return value;

        if (value.Length <= limit) return value;

        return value.Substring(0, limit);
    }

    public static int ToIntDefault(this string value, int defaultValue)
    {
        try
        {
            return int.Parse(value);
        }
        catch 
        { 
            return defaultValue; 
        }
    }
}