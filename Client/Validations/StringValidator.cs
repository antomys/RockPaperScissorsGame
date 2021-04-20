using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Client.Validations
{
    public static class StringValidator
    {
        public static bool IsStringContainsDigits(string str)
        {
            var res = str.Any<char>(ch => Char.IsDigit(ch));
            return res;
        }
        public static bool IsEmailValid(string email)
        { 
            var emailPattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            var match = Regex.Match(email,emailPattern,RegexOptions.IgnoreCase);
            return match.Success;
        }
    }
}
