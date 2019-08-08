using System;
using System.Collections.Generic;
using Wordifier.Services.Currency.Helpers;

namespace Wordifier.Services.Currency
{
    public class CurrencyWordifier : IWordifier
    {
        private readonly DigitGroupEnum[] _groups;
        private static readonly ParsedNumber Zero  = new ParsedNumber
        {
            Group = DigitGroupEnum.None,
            Value = 0
        };

        public CurrencyWordifier()
        {
            // this should follow a strict increasing order of groups
            _groups = new []
            {
                DigitGroupEnum.None,
                DigitGroupEnum.Thousand,
                DigitGroupEnum.Million,
                DigitGroupEnum.Billion
            };
        }

        public decimal Validate(string num)
        {
            if (!decimal.TryParse(num, out var number)) { throw new ArgumentException("Invalid argument value (expected decimal)."); }
            if (number <= 0) { throw new ArgumentException("Invalid argument value (expected positive number)."); }
            if (number > 2000000000) { throw new ArgumentException("Invalid argument value (expected less than 2 billion)."); }

            NumberParser.GetWholeNumber(number, out var remainder);
            if (remainder % 1 > 0) { throw new ArgumentException("Invalid argument value (expected at most two decimal places)."); }

            return number;
        }

        public string Convert(decimal num)
        {
            var wholeNumber = NumberParser.GetWholeNumber(num, out var remainder);
            // parse numbers into usable format
            var groups = GetGroups(wholeNumber);

            var dollarString = string.Empty;
            if(wholeNumber > 0) { dollarString = wholeNumber > 1 ? "dollars" : "dollar"; }
            var wholeNumberString = GetString(groups);

            var centsString = remainder == 1 ? "cent" : "cents";
            var andString = wholeNumber > 0 ? "and" : string.Empty;
            var remainderGroup = GetGroups(remainder, includeZero: true);
            var remainderString = GetString(remainderGroup, includeZero: true);
            remainderString = $"{andString} {remainderString}".Trim();

            return $"{wholeNumberString} {dollarString} {remainderString} {centsString}".Trim();
        }

        private ParsedNumber[] GetGroups(decimal value, bool includeZero = false)
        {
            // separate the value into groups of 1000
            var list = new List<ParsedNumber>();
            var divisor = 1000;
            var remainingValue = value;
            var groupIndex = 0;

            if(value == 0 && includeZero) { return new [] { Zero }; }

            while(remainingValue > 0)
            {
                // get the group
                var remainder = remainingValue % divisor;

                var parsed = new ParsedNumber
                {
                    Group = _groups[groupIndex],
                    Value = (int)(remainder % 1000)
                };
                // insert at the front to retain the order of groups in the value (left to right)
                // this is because the processing starts from right of the value to the left
                list.Insert(0, parsed);

                // remove the current group from the value
                groupIndex++;
                remainingValue = (remainingValue - remainder) / divisor;
            }

            return list.ToArray();
        }

        private string GetString(ParsedNumber[] groups, bool includeZero = false)
        {
            // get the string representation of each group
            var strings = new List<string>();
            foreach(var item in groups)
            {
                if(item.Value == 0 && !includeZero) { continue; }
                var valueString = NumberParser.GetGroupString(item.Value, item.Group, includeZero: includeZero, includeAnd: true);
                strings.Add(valueString);
            }
            return string.Join(", ", strings);
        }
    }
}