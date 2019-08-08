using System;
using System.Linq;
using Wordifier.Services;

namespace Wordifier
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args?.Any() != true) { throw new ArgumentException("Missing argument."); }
            if (args.Length != 1) { throw new ArgumentException("Invalid argument."); }

            IWordifierService wordifier = new WordifierService();
            var output = wordifier.Wordify(args[0], WordifierTypeEnum.Currency);
            Console.WriteLine(output);
        }
    }
}
