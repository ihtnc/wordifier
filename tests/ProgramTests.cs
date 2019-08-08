using Wordifier;
using System;
using Xunit;
using FluentAssertions;

namespace Wordifier.Tests
{
    public class ProgramTests
    {
        [Theory]
        [InlineData(null, "Missing argument.")]
        [InlineData(new string[0], "Missing argument.")]
        [InlineData(new [] {"1", "2"}, "Invalid argument.")]
        public void Main_Should_Validate(string[] args, string expectedMessage)
        {
            Action action = () => { Program.Main(args); };
            action.Should().Throw<ArgumentException>()
                .Which.Message.Should().Be(expectedMessage);
        }
    }
}