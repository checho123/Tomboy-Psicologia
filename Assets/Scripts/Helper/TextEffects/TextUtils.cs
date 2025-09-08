using System;
using System.Text.RegularExpressions;

public static class TextUtils
{
    // Cuenta palabras separadas por espacios (simple y r�pido)
    public static int CountWords(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return 0;
        var parts = s.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length;
    }

    // Versi�n estricta: cuenta solo secuencias de letras (ignora signos y n�meros)
    public static int CountWordsStrict(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return 0;
        return Regex.Matches(s, @"\p{L}+").Count;
    }

    // Limita a N palabras y a�ade sufijo (p. ej., "�")
    public static string LimitWords(string s, int maxWords, string suffix = "�")
    {
        if (string.IsNullOrWhiteSpace(s) || maxWords <= 0) return string.Empty;
        var parts = s.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length <= maxWords) return s;
        return string.Join(" ", parts, 0, maxWords) + suffix;
    }
}
