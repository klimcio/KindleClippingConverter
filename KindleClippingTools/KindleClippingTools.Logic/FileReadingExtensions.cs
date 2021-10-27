using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("KindleClippingTools.Tests")]
namespace KindleClippingTools.Logic
{
    internal static class FileReadingExtensions
    {
        private const string ClippingSeparator = "==========";
        private const string LineSectionSeparator = "|";

        internal static string[] DivideRawFileIntoRawNotes(this string fileContent)
        {
            return fileContent.Split(ClippingSeparator);
        }

        internal static Clipping ConvertToClippings(this string rawClipping)
        {
            if (string.IsNullOrWhiteSpace(rawClipping))
                return null;

            var clippingLines = rawClipping
                .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (clippingLines.Count == 1)
                return null;

            var line2 = clippingLines[1].Split(LineSectionSeparator);

            return new Clipping(
                title: clippingLines[0].Trim(),
                line2[0].RecognizeClippingType(),
                line2[0].ExtractPageNumber(),
                line2[1].ExtractLocationStart(),
                line2[2].ExtractDate(),
                line2[1].ExtractLocationEnd(),
                content: clippingLines.ExtractContent()
            );

        }

        internal static DateTime ExtractDate(this string line2section3)
        {
            var dateString = line2section3.Substring(line2section3.IndexOf(",")).Trim();

            return DateTime.Parse(dateString, new CultureInfo("en-US"));
        }

        internal static int? ExtractLocationEnd(this string line2section2)
        {
            var dashIndex = line2section2.IndexOf("-");
            if (dashIndex == -1)
                return null;

            var locationEndString = line2section2.Substring(dashIndex + 1);

            return Convert.ToInt32(locationEndString);
        }

        internal static int ExtractLocationStart(this string line2section2)
        {
            var dashIndex = line2section2.IndexOf("-");
            int startIndex = line2section2.IndexOf("location") + "location".Length;

            string locationStartString;
            if (dashIndex == -1)
            {
                locationStartString = line2section2.Substring(startIndex).Trim();
            }
            else
            {
                var length = dashIndex - startIndex;
                locationStartString = line2section2.Substring(startIndex, length).Trim();
            }

            return Convert.ToInt32(locationStartString);
        }

        internal static int ExtractPageNumber(this string line2section1)
        {
            var pageWordIdx = line2section1.Trim().IndexOf("page");
            var pageNoString = line2section1.Substring(pageWordIdx + 4).Trim();

            return Convert.ToInt32(pageNoString);
        }

        internal static string ExtractContent(this IEnumerable<string> clippingLines)
        {
            StringBuilder content = new();
            for (var i = 2; i < clippingLines.Count(); i++)
            {
                var contentLine = clippingLines.ElementAt(i);

                if (string.IsNullOrWhiteSpace(contentLine))
                    continue;

                content.AppendLine(contentLine);
            }

            return content.ToString();
        }

        internal static ClippingType RecognizeClippingType(this string line2section1)
        {
            if (line2section1.Contains("Highlight"))
                return ClippingType.Highlight;

            if (line2section1.Contains("Note"))
                return ClippingType.Note;

            return ClippingType.Bookmark;
        }
    }
}
