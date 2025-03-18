using FluentAssertions;
using KindleClippingTools.Logic.Extensions;
using Xunit;

namespace KindleClippingTools.Tests
{
    public class ExtractingLocationNumbersTests
    {
        [Theory]
        [InlineData("- Highlight Loc. 687702-3  ", 687702, 3)]
        [InlineData(" Loc. 73-74  ", 73, 74)]
        [InlineData(" Loc. 74  ", 74, null)]
        [InlineData("- Bookmark Loc. 687703  ", 687703, null)]
        public void ExtractingLocations(string locationString, int expectedStart, int? expectedEnd)
        {
            var locationStart = locationString.ExtractLocationStart();
            var locationEnd = locationString.ExtractLocationEnd();

            locationStart.Should().Be(expectedStart);
            locationEnd.Should().Be(expectedEnd);
        }
    }
}
