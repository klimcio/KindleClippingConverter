using KindleClippingTools.Logic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                .Select(file => converter.ParseFile(file));

            ;
        }
    }
}
