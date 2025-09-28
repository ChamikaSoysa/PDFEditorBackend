using System.Net;

namespace PDFBackend.Extensions
{
    public static class Sanitizer
    {
        public static string SanitizeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            return WebUtility.HtmlEncode(input.Trim());
        }

        public static string SanitizeFileName(string fileName)
        {
            var invalid = Path.GetInvalidFileNameChars();
            return string.Join("_", fileName.Split(invalid, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }
    }
}
