using System.Globalization;

namespace Bank.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToSwedishKrona(this decimal value)
        {
            var nfi = (NumberFormatInfo)CultureInfo.GetCultureInfo("sv-SE").NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            return value.ToString("N", nfi) + " kr";
        }

        public static string ToFirstLetterUpper(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return input.Substring(0, 1).ToUpper() + input.Substring(1, input.Length - 1).ToLower();
        }
    }
}
