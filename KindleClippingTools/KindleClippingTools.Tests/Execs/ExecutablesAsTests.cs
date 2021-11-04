using KindleClippingTools.Logic;
using KindleClippingTools.Logic.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace KindleClippingTools.Tests.Execs
{
    public class ExecutablesAsTests
    {
        private string[] GetFiles() 
            => Directory.GetFiles(@"C:\Users\MAKLIM\Dropbox\Projekty\clippings");

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

            clippings.BackupKindleClippings(targetDirectory);
        }
    }
}
