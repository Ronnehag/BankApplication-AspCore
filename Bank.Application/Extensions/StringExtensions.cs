using System.Globalization;

namespace Bank.Application.Extensions
{
    public static class StringExtensions
    {
        public static string ToSwedishKrona(this decimal value)
        {
            var nfi = (NumberFormatInfo) CultureInfo.GetCultureInfo("sv-SE").NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            return value.ToString("N", nfi) + " kr";
        }
    }
}
