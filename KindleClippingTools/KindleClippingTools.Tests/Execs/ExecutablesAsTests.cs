using KindleClippingTools.Logic;
using KindleClippingTools.Logic.Extensions;
using System.IO;
using System.Linq;
using Xunit;

namespace KindleClippingTools.Tests.Execs
{
    public class ExecutablesAsTests
    {
        [Fact]
        public void RunClippingConverter()
        {
            var targetDirectory = @"C:\_Temp\Kindle";
            var sourceDirectory = @"C:\Users\MAKLIM\Dropbox\Projekty\clippings";
            
            var converter = new KindleClippingParser();

            Directory.GetFiles(sourceDirectory)
                .SelectMany(file => converter.ParseFile(file))
                .Where(x => x.Type != ClippingType.Bookmark)
                .GroupBy(x => x.Title)
                .BackupKindleClippings(targetDirectory);
        }
    }
}
