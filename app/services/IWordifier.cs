namespace Wordifier.Services
{
    public interface IWordifier
    {
        decimal Validate(string num);
        string Convert(decimal num);
    }
}