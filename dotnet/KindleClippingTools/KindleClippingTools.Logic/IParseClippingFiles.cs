using System.Collections.Generic;

namespace KindleClippingTools.Logic
{
    public interface IParseClippingFiles
    {
        List<Clipping> ParseFile(string path);
    }
}
