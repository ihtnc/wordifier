namespace Wordifier.Services.Currency.Helpers
{
    public static class NumberParser
    {
        public static decimal GetWholeNumber(decimal value, out decimal remainder, int decimalPlaces = 2)
        {
            var offset = 1;
            for(var i = 0; i < decimalPlaces; i++) { offset *= 10; }

            var remainderNumber = value % 1;
            var wholeNumber = value - remainderNumber;
            remainder = remainderNumber * offset;
            return wholeNumber;
        }

        public static string GetGroupString(int value, DigitGroupEnum group, bool includeZero = false, bool includeAnd = false)
        {
            var hundredsDivisor = 1000;
            var tensDivisor = 100;
            var onesDivisor = 10;

            var hundreds = (byte)((value % hundredsDivisor) / tensDivisor);
            var hundredsString = GetHundredsDigitString(hundreds);

            var ones = (byte)((value % onesDivisor));
            var tens = (byte)((value % tensDivisor) / onesDivisor);
            includeZero = includeZero && hundreds == 0;
            var tensString = GetTensDigitString(tens, ones, includeZero);

            // only add "and" if there is a hundreds digit and either a ones or tens digit
            var andString = includeAnd && hundreds > 0 && (tens > 0 || ones > 0) ? " and" : string.Empty;
            var valueString = $"{hundredsString}{andString} {tensString}".Trim();

            var groupString = string.Empty;
            switch(group)
            {
                case DigitGroupEnum.Thousand:
                    groupString = "thousand";
                    break;
                case DigitGroupEnum.Million:
                    groupString = "million";
                    break;
                case DigitGroupEnum.Billion:
                    groupString = "billion";
                    break;
            }

            // only add the group string if there is value
            groupString = value > 0 ? groupString : string.Empty;

            return $"{valueString} {groupString}".Trim();
        }

        public static string GetOnesDigitString(byte value, bool specialCase = false, bool includeZero = false)
        {
            var output = string.Empty;
            switch(value)
            {
                case 1:
                    output =  specialCase ? "eleven" : "one";
                    break;
                case 2:
                    output =  specialCase ? "twelve" : "two";
                    break;
                case 3:
                    output =  specialCase ? "thirteen" : "three";
                    break;
                case 4:
                    output =  specialCase ? "fourteen" : "four";
                    break;
                case 5:
                    output =  specialCase ? "fifteen" : "five";
                    break;
                case 6:
                    output =  specialCase ? "sixteen" : "six";
                    break;
                case 7:
                    output =  specialCase ? "seventeen" : "seven";
                    break;
                case 8:
                    output =  specialCase ? "eighteen" : "eight";
                    break;
                case 9:
                    output =  specialCase ? "nineteen" : "nine";
                    break;
                case 0:
                    output =  specialCase ? "ten" : includeZero ? "zero" : string.Empty;
                    break;
            }

            return output;
        }

        public static string GetTensDigitString(byte tens, byte ones, bool includeZero = false)
        {
            var output = string.Empty;
            switch(tens)
            {
                case 1:
                    output = $"{GetOnesDigitString(ones, specialCase: true)}";
                    break;
                case 2:
                    output = $"twenty {GetOnesDigitString(ones)}";
                    break;
                case 3:
                    output = $"thirty {GetOnesDigitString(ones)}";
                    break;
                case 4:
                    output = $"forty {GetOnesDigitString(ones)}";
                    break;
                case 5:
                    output = $"fifty {GetOnesDigitString(ones)}";
                    break;
                case 6:
                    output = $"sixty {GetOnesDigitString(ones)}";
                    break;
                case 7:
                    output = $"seventy {GetOnesDigitString(ones)}";
                    break;
                case 8:
                    output = $"eighty {GetOnesDigitString(ones)}";
                    break;
                case 9:
                    output = $"ninety {GetOnesDigitString(ones)}";
                    break;
                case 0:
                    output = $"{GetOnesDigitString(ones, includeZero: includeZero)}";
                    break;
            }

            return output.Trim();
        }

        public static string GetHundredsDigitString(byte value)
        {
            var hundredsString = value > 0 ? "hundred" : string.Empty;
            return $"{GetOnesDigitString(value)} {hundredsString}".Trim();
        }
    }
}