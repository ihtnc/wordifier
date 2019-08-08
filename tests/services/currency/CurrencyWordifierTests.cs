using System;
using Wordifier.Services;
using Wordifier.Services.Currency;
using Xunit;
using FluentAssertions;

namespace Wordifier.Tests.Services.Currency
{
    public class CurrencyWordifierTests
    {
        private readonly IWordifier _wordifier;

        public CurrencyWordifierTests()
        {
            _wordifier = new CurrencyWordifier();
        }

        [Theory]
        [InlineData("123a", "Invalid argument value (expected decimal).")]
        [InlineData("string", "Invalid argument value (expected decimal).")]
        [InlineData("2019-01-04", "Invalid argument value (expected decimal).")]
        [InlineData("127.0.0.1", "Invalid argument value (expected decimal).")]
        [InlineData("-4", "Invalid argument value (expected positive number).")]
        [InlineData("2000000000.01", "Invalid argument value (expected less than 2 billion).")]
        [InlineData("123.001", "Invalid argument value (expected at most two decimal places).")]
        public void Validate_Should_Throw_Exception(string value, string expectedMessage)
        {
            Action action = () => { _wordifier.Validate(value); };
            action.Should().Throw<ArgumentException>()
                .Which.Message.Should().Be(expectedMessage);
        }

        [Theory]
        [InlineData("123", 123)]
        [InlineData("123.4", 123.4)]
        [InlineData("123.00", 123)]
        [InlineData("12345.67", 12345.67)]
        [InlineData("1,234,567", 1234567)]
        [InlineData(".23", 0.23)]
        [InlineData("0.12", 0.12)]
        public void Validate_Should_Return_Correctly(string value, decimal expectedValue)
        {
            _wordifier.Validate(value).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(1, "one dollar and zero cents")]
        [InlineData(.5, "fifty cents")]
        [InlineData(12.01, "twelve dollars and one cent")]
        [InlineData(100.18, "one hundred dollars and eighteen cents")]
        [InlineData(10707.10, "ten thousand, seven hundred and seven dollars and ten cents")]
        [InlineData(900000, "nine hundred thousand dollars and zero cents")]
        [InlineData(998012.01, "nine hundred and ninety eight thousand, twelve dollars and one cent")]
        [InlineData(1013030.40, "one million, thirteen thousand, thirty dollars and forty cents")]
        [InlineData(19101222.33, "nineteen million, one hundred and one thousand, two hundred and twenty two dollars and thirty three cents")]
        [InlineData(419004000.74, "four hundred and nineteen million, four thousand dollars and seventy four cents")]
        [InlineData(1000100005.00, "one billion, one hundred thousand, five dollars and zero cents")]
        [InlineData(1234567899.11, "one billion, two hundred and thirty four million, five hundred and sixty seven thousand, eight hundred and ninety nine dollars and eleven cents")]
        public void Convert_Should_Return_Correctly(decimal num, string expectedResponse)
        {
            _wordifier.Convert(num).Should().Be(expectedResponse);
        }
    }
}