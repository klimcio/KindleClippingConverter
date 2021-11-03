using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KindleClippingTools.Tests")]
namespace KindleClippingTools.Logic
{
    internal static class StringExtensions
    {
        internal static string ConvertStringToFolderName(this string someString)
        {
            return someString
                .Replace(":", "")
                .Replace("?", "");
        }
    }
}
