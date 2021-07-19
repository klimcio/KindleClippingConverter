using System;

namespace KindleClippingTools.Logic
{
    public class Clipping
    {
        public string Title { get; set; }
        public ClippingType Type { get; set; }
        public int PageNumber { get; set; }
        public int LocationStart { get; set; }
        public int LocationEnd { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Content { get; set; }
    }
}
