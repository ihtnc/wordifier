using Wordifier.Services;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace Wordifier.Tests.Services
{
    public class WordifierServiceTests
    {
        private readonly IWordifierProvider _provider;
        private readonly IWordifierService _service;

        public WordifierServiceTests()
        {
            _provider = Substitute.For<IWordifierProvider>();
            _service = new WordifierService(_provider);
        }

        [Fact]
        public void Wordify_Should_Call_IWordifierProvider_GetWordifier()
        {
            var type = WordifierTypeEnum.Currency;

            _service.Wordify("any", type);

            _provider.Received(1).GetWordifier(type);
        }

        [Fact]
        public void Wordify_Should_Call_IWordifier_Validate()
        {
            var wordifier = Substitute.For<IWordifier>();
            _provider.GetWordifier(Arg.Any<WordifierTypeEnum>()).Returns(wordifier);

            var value = "any";
            _service.Wordify(value, WordifierTypeEnum.Currency);

            wordifier.Received(1).Validate(value);
        }

        [Fact]
        public void Wordify_Should_Call_IWordifier_Convert()
        {
            var num = 123;
            var wordifier = Substitute.For<IWordifier>();
            wordifier.Validate(Arg.Any<string>()).Returns(num);
            _provider.GetWordifier(Arg.Any<WordifierTypeEnum>()).Returns(wordifier);

            _service.Wordify("any", WordifierTypeEnum.Currency);

            wordifier.Received(1).Convert(num);
        }

        [Fact]
        public void Wordify_Should_Return_Correctly()
        {
            var response = "any";
            var wordifier = Substitute.For<IWordifier>();
            wordifier.Convert(Arg.Any<decimal>()).Returns(response);
            _provider.GetWordifier(Arg.Any<WordifierTypeEnum>()).Returns(wordifier);

            _service.Wordify("any", WordifierTypeEnum.Currency).Should().Be(response);
        }
    }
}