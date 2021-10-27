using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KindleClippingTools.Logic
{
    public class KindleClippingParser : IParseClippingFiles
    {
        public List<Clipping> ParseFile(string path)
        {
            return File.ReadAllText(path)
                .DivideRawFileIntoRawNotes()
                .Select(x => x.ConvertToClippings())
                .Where(x => x != null)
                .ToList();
        }
    }
}
