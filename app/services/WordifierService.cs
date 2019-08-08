namespace Wordifier.Services
{
    public interface IWordifierService
    {
        string Wordify(string numericValue, WordifierTypeEnum wordifierType);
    }

    public class WordifierService : IWordifierService
    {
        private readonly IWordifierProvider _provider;

        public WordifierService(IWordifierProvider provider = null)
        {
            _provider = provider ?? new WordifierProvider();
        }

        public string Wordify(string numericValue, WordifierTypeEnum wordifierType)
        {
            var wordifier = _provider.GetWordifier(wordifierType);
            var num = wordifier.Validate(numericValue);
            return wordifier.Convert(num);
        }
    }
}