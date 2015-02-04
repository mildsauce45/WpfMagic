using System;
using System.Text;

namespace BddSharp.Engine.Extensions
{
    public static class StringExtensions
    {
        public static string SeparateOnCamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var sb = new StringBuilder();

            foreach (var c in str)
            {
                if (Char.IsUpper(c) && sb.Length > 0)                
                    sb.Append(" ");

                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
