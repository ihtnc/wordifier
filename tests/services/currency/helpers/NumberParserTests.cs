using Wordifier.Services.Currency;
using Wordifier.Services.Currency.Helpers;
using Xunit;
using FluentAssertions;

namespace Wordifier.Tests.Services.Currency.Helpers
{
    public class NumberParserTests
    {
        [Theory]
        [InlineData(123.45, 1, 123, 4.5)]
        [InlineData(123.45, 2, 123, 45)]
        [InlineData(123.45, 3, 123, 450)]
        [InlineData(123.45, 0, 123, .45)]
        [InlineData(.45, 4, 0, 4500)]
        [InlineData(.01, 4, 0, 100)]
        [InlineData(.01, 3, 0, 10)]
        [InlineData(.01, 2, 0, 1)]
        [InlineData(.01, 0, 0, 0.01)]
        public void GetWholeNumber_Should_Separate_Whole_And_Fractional_Component(decimal value, int decimalPlaces, decimal expectedWholeNumber, decimal expectedDecimalNumber)
        {
            decimal actualDecimalNumber;
            var actualWholeNumber = NumberParser.GetWholeNumber(value, out actualDecimalNumber, decimalPlaces);

            actualWholeNumber.Should().Be(expectedWholeNumber);
            actualDecimalNumber.Should().Be(expectedDecimalNumber);
        }

        [Theory]
        [InlineData(123.45, 123, 45)]
        [InlineData(1235, 1235, 0)]
        [InlineData(32.1, 32, 10)]
        [InlineData(5432.01, 5432, 1)]
        public void GetWholeNumber_Should_Handle_Default_DecimalPlaces_To_2(decimal value, decimal expectedWholeNumber, decimal expectedDecimalNumber)
        {
            decimal actualDecimalNumber;
            var actualWholeNumber = NumberParser.GetWholeNumber(value, out actualDecimalNumber);

            actualWholeNumber.Should().Be(expectedWholeNumber);
            actualDecimalNumber.Should().Be(expectedDecimalNumber);
        }

        [Theory]
        [InlineData(1, DigitGroupEnum.None, true, false, "one")]
        [InlineData(14, DigitGroupEnum.Thousand, false, false, "fourteen thousand")]
        [InlineData(20, DigitGroupEnum.Million, true, false, "twenty million")]
        [InlineData(356, DigitGroupEnum.Billion, true, false, "three hundred fifty six billion")]
        [InlineData(789, DigitGroupEnum.None, false, true, "seven hundred and eighty nine")]
        [InlineData(55, DigitGroupEnum.Thousand, true, true, "fifty five thousand")]
        [InlineData(600, DigitGroupEnum.Million, true, true, "six hundred million")]
        [InlineData(0, DigitGroupEnum.Billion, true, true, "zero")]
        [InlineData(0, DigitGroupEnum.Thousand, true, false, "zero")]
        [InlineData(0, DigitGroupEnum.None, true, true, "zero")]
        [InlineData(0, DigitGroupEnum.None, true, false, "zero")]
        public void GetGroupString_Should_Return_Correctly(int value, DigitGroupEnum group, bool includeZero, bool includeAnd, string expected)
        {
            NumberParser.GetGroupString(value, group, includeZero, includeAnd).Should().Be(expected);
        }

        [Theory]
        [InlineData(1, false, true, "one")]
        [InlineData(1, false, false, "one")]
        [InlineData(2, false, true, "two")]
        [InlineData(2, false, false, "two")]
        [InlineData(3, false, true, "three")]
        [InlineData(3, false, false, "three")]
        [InlineData(4, false, true, "four")]
        [InlineData(4, false, false, "four")]
        [InlineData(5, false, true, "five")]
        [InlineData(5, false, false, "five")]
        [InlineData(6, false, true, "six")]
        [InlineData(6, false, false, "six")]
        [InlineData(7, false, true, "seven")]
        [InlineData(7, false, false, "seven")]
        [InlineData(8, false, true, "eight")]
        [InlineData(8, false, false, "eight")]
        [InlineData(9, false, true, "nine")]
        [InlineData(9, false, false, "nine")]
        [InlineData(0, false, true, "zero")]
        [InlineData(0, false, false, "")]
        [InlineData(1, true, true, "eleven")]
        [InlineData(2, true, true, "twelve")]
        [InlineData(3, true, true, "thirteen")]
        [InlineData(4, true, true, "fourteen")]
        [InlineData(5, true, true, "fifteen")]
        [InlineData(6, true, true, "sixteen")]
        [InlineData(7, true, true, "seventeen")]
        [InlineData(8, true, true, "eighteen")]
        [InlineData(9, true, true, "nineteen")]
        [InlineData(0, true, true, "ten")]
        [InlineData(0, true, false, "ten")]
        [InlineData(11, false, true, "")]
        [InlineData(12, false, false, "")]
        [InlineData(13, true, true, "")]
        [InlineData(14, true, false, "")]
        public void GetOnesDigitString_Should_Return_Correctly(byte value, bool specialCase, bool includeZero, string expected)
        {
            NumberParser.GetOnesDigitString(value, specialCase, includeZero).Should().Be(expected);
        }

        [Fact]
        public void GetOnesDigitString_Should_Default_SpecialCase_To_False()
        {
            NumberParser.GetOnesDigitString(2).Should().Be("two");
        }

        [Fact]
        public void GetOnesDigitString_Should_Default_IncludeZero_To_False()
        {
            NumberParser.GetOnesDigitString(0).Should().Be("");
        }

        [Theory]
        [InlineData(1, 1, true, "eleven")]
        [InlineData(2, 3, false, "twenty three")]
        [InlineData(1, 0, true, "ten")]
        [InlineData(5, 0, false, "fifty")]
        [InlineData(0, 0, true, "zero")]
        [InlineData(0, 0, false, "")]
        [InlineData(0, 4, true, "four")]
        [InlineData(0, 8, false, "eight")]
        [InlineData(12, 6, false, "")]
        [InlineData(99, 7, true, "")]
        public void GetTensDigitString_Should_Return_Correctly(byte tensValue, byte onesValue, bool includeZero, string expected)
        {
            NumberParser.GetTensDigitString(tensValue, onesValue, includeZero).Should().Be(expected);
        }

        [Fact]
        public void GetTensDigitString_Should_Default_Include_Zero_To_False()
        {
            NumberParser.GetTensDigitString(0, 0).Should().Be("");
        }

        [Theory]
        [InlineData(1, "one hundred")]
        [InlineData(8, "eight hundred")]
        [InlineData(0, "")]
        public void GetHundredsDigitString(byte value, string expected)
        {
            NumberParser.GetHundredsDigitString(value).Should().Be(expected);
        }
    }
}
