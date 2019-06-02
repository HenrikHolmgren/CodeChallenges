using System;
using System.Text;

namespace WindowsillSoft.CodeChallenges.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToProperName(this string camelCaseName)
        {
            var result = new StringBuilder();
            for (var i = 0; i < camelCaseName.Length; i++)
            {
                if (Char.IsUpper(camelCaseName[i]))
                    result.Append(" ");
                result.Append(camelCaseName[i]);
            }
            return result.ToString();
        }
    }
}
