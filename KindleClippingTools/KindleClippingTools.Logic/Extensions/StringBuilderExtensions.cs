using System;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("KindleClippingTools.Tests")]
namespace KindleClippingTools.Logic.Extensions
{
    internal static class StringBuilderExtensions
    {
        internal static void AppendContent(this StringBuilder builder, Clipping clipping)
        {
            if (clipping.IsHighlight)
            {
                builder.AppendLine($"> {clipping.Content}");
            }
            else
            {
                builder.AppendLine(clipping.Content);
            }

            builder.AppendLine();
        }

        internal static void AddClippingData(this StringBuilder builder, DateTime createdOn)
        {
            builder.AppendLine($"Created on: {createdOn}");
        }

        internal static void AddBookReference(this StringBuilder builder, string bookTitle)
        {
            builder.AppendLine(string.Format("[[{0}]]", bookTitle));
        }
    }
}
