using System;
using Wordifier.Services;
using Wordifier.Services.Currency;
using Xunit;
using FluentAssertions;

namespace Wordifier.Tests.Services
{
    public class WordifierProviderTests
    {
        private readonly IWordifierProvider _provider;

        public WordifierProviderTests()
        {
            _provider = new WordifierProvider();
        }

        [Theory]
        [InlineData(WordifierTypeEnum.Currency, typeof(CurrencyWordifier))]
        public void GetWordifier_Should_Return_Correctly(WordifierTypeEnum typeEnum, Type expectedWordifier)
        {
            _provider.GetWordifier(typeEnum).Should().BeOfType(expectedWordifier);
        }
    }
}