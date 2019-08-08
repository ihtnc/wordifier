using System;

namespace Wordifier.Services
{
    public interface IWordifierProvider
    {
        IWordifier GetWordifier(WordifierTypeEnum wordifierType);
    }

    public class WordifierProvider : IWordifierProvider
    {
        public IWordifier GetWordifier(WordifierTypeEnum wordifierType)
        {
            switch(wordifierType)
            {
                case WordifierTypeEnum.Currency:
                    return new Currency.CurrencyWordifier();
                default:
                    throw new ArgumentException($"Unsupported type {wordifierType}.");
            }
        }
    }
}