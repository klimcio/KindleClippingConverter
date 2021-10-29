using KindleClippingTools.Logic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace KindleClippingTools.Tests.Execs
{
    public class ExecutablesAsTests
    {
        private string[] GetFiles()
        {
            var directory = @"C:\Users\MAKLIM\Dropbox\Projekty\clippings";

            return Directory.GetFiles(directory);
        }

        [Fact]
        public void RunClippingConverter()
        {
            var filesToConvert = GetFiles();

            var converter = new KindleClippingParser();
            var list = new List<Clipping>();

            var clippings = GetFiles()
                .SelectMany(file => converter.ParseFile(file))
                .Where(x => x.Type != ClippingType.Bookmark)
                .GroupBy(x => x.Title);

            var targetDirectory = @"C:\_Temp\Kindle";

            foreach (var group in clippings)
            {
                var bookTitle = group.Key
                    .Replace(":", "")
                    .Replace("?", "");

                var dirInfo = Directory.CreateDirectory(Path.Combine(targetDirectory, bookTitle));

                foreach(var clipping in group)
                {
                    var builder = new StringBuilder();
                    bool isHighlight = clipping.Type == ClippingType.Highlight;

                    var fileName = string.Format("{0}_{1}_{2}.md", isHighlight ? "H" : "N", clipping.PageNumber, clipping.LocationStart);

                    if (isHighlight)
                    {
                        builder.AppendLine(clipping.Content);
                    }
                    else
                    {
                        builder.AppendLine();
                        builder.AppendLine(clipping.Content);
                        builder.AppendLine();
                    }

                    string path = Path.Combine(dirInfo.FullName, fileName);
                    if (File.Exists(path))
                    {
                        ;
                    }

                    File.WriteAllText(path, builder.ToString());
                }
            }
        }
    }
}
