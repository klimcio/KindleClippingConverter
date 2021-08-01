using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace KindleClippingTools.Logic
{
    public class KindleClippingParser : IParseClippingFiles
    {
        public List<Clipping> ParseFile(string path)
        {
            var rawClippings = File.ReadAllText(path).Split("==========");

            if (rawClippings.Length == 1 && string.IsNullOrWhiteSpace(rawClippings[0]))
                return new List<Clipping>();

            List<Clipping> clippings = new();

            foreach(var rawClipping in rawClippings)
            {
                if (string.IsNullOrWhiteSpace(rawClipping))
                    continue;

                clippings.Add(Create(rawClipping));
            }

            return clippings.Where(x => x != null).ToList();
        }

        public Clipping Create(string clipping)
        {
            var clippingLines = clipping.Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (clippingLines.Count == 1)
                return null;

            var line2 = clippingLines[1].Split("|");

            return new Clipping(
                title: clippingLines[0].Trim(),
                RecognizeClippingType(line2[0]),
                ExtractPageNumber(line2[0]),
                ExtractLocationStart(line2[1]),
                ExtractDate(line2[2]),
                ExtractLocationEnd(line2[1]),
                content: ExtractContent(clippingLines).ToString()
            );
        }

        private DateTime ExtractDate(string line2section3)
        {
            var dateString = line2section3.Substring(line2section3.IndexOf(",")).Trim();

            return DateTime.Parse(dateString, new CultureInfo("en-US"));
        }

        private int? ExtractLocationEnd(string line2section2)
        {
            var dashIndex = line2section2.IndexOf("-");
            if (dashIndex == -1)
                return null;

            var locationEndString = line2section2.Substring(dashIndex + 1);

            return Convert.ToInt32(locationEndString);
        }

        private int ExtractLocationStart(string line2section2)
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

        private int ExtractPageNumber(string line2section1)
        {
            var pageWordIdx = line2section1.Trim().IndexOf("page");
            var pageNoString = line2section1.Substring(pageWordIdx + 4).Trim();

            return Convert.ToInt32(pageNoString);
        }

        private string ExtractContent(IEnumerable<string> clippingLines)
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

        private ClippingType RecognizeClippingType(string line2section1)
        {
            if (line2section1.Contains("Highlight"))
                return ClippingType.Highlight;

            if (line2section1.Contains("Note"))
                return ClippingType.Note;

            return ClippingType.Bookmark;
        }
    }
}
