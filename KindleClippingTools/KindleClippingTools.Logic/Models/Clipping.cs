using System;

namespace KindleClippingTools.Logic
{
    public class Clipping
    {
        public Clipping(string title, 
            ClippingType type, 
            int? pageNumber, 
            int locationStart, 
            DateTime createdOn, 
            int? locationEnd = null, 
            string content = "")
        {
            Title = title;
            Type = type;
            PageNumber = pageNumber;
            LocationStart = locationStart;
            LocationEnd = locationEnd;
            CreatedOn = createdOn;
            Content = content;
        }

        public string Title { get; }
        public ClippingType Type { get; }
        public int? PageNumber { get; }
        public int LocationStart { get; }
        public int? LocationEnd { get; }
        public DateTime CreatedOn { get; }
        public string Content { get; }
    }
}
