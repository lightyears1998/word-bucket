using System.Collections.Generic;

namespace WordBucket.Services
{
    public static class WordService
    {
        public static IEnumerable<string> TryLemmatize(string word)
        {
            HashSet<string> result = new();

            if (word.EndsWith("ed"))
            {
                result.Add(word.RemoveSuffix("ed"));
                result.Add(word.RemoveSuffix("d"));
            }

            if (word.EndsWith("s"))
            {
                result.Add(word.RemoveSuffix("s"));
            }

            if (word.EndsWith("es"))
            {
                result.Add(word.RemoveSuffix("es"));
            }

            return result;
        }

        private static string RemoveSuffix(this string word, string suffix)
        {
            return word[..^suffix.Length];
        }
    }
}
