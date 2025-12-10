namespace Sisa.Panel.Extensions
{
    internal static class StringExtensions
    {
        extension (string str)
        {
            public bool EqualsOrdinal(string str1)
            {
                return str.Equals(str1, StringComparison.Ordinal);
            }

            public bool ContainsOrdinal(string str1)
            {
                return str.Contains(str1, StringComparison.Ordinal);
            }

            public bool StartsWithOrdinal(string str1)
            {
                return str.StartsWith(str1, StringComparison.Ordinal);
            }
        }
    }
}
