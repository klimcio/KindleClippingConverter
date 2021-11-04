using FluentAssertions;
using KindleClippingTools.Logic.Extensions;
using Xunit;

namespace KindleClippingTools.Tests
{
    public class ExtractingPageNumberTests
    {
        [Theory]
        [InlineData("- Highlight on Page 5")]
        public void ExtractPageNumber(string sectionString)
        {
            var result = sectionString.ExtractPageNumber();

            result.Should().Be(5);
        }

        [Theory]
        [InlineData("- Highlight Loc. 687702-3")]
        public void ReturnsNullWhenPageIsNotProvided(string sectionString)
        {
            var result = sectionString.ExtractPageNumber();

            result.Should().BeNull();
        }
    }
}
