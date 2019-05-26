using Bank.Common.Extensions;
using Shouldly;
using Xunit;

namespace Bank.Application.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact(DisplayName = "StringExtensions_FirstLetterUpperCase")]
        public void FirstLetterUpper_ValidString_ShouldReturnFirstUpper()
        {
            // Assert
            var test = "helLo WorlD";

            // Act
            var result = test.ToFirstLetterUpper();

            // Assert
            result.ShouldBe("Hello world");
        }
    }
}
