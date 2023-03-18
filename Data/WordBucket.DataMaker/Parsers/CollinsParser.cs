using WordBucket.Contexts;
using WordBucket.Models;

namespace WordBucket.DataMaker.Parsers
{
    internal static class CollinsParser
    {
        public static void Parse(string baseDirectory)
        {
            using DictionaryContext db = new();

            for (int level = 0; level <= 5; ++level)
            {
                var filePath = Path.Join(baseDirectory, $"Collins_{level}.txt");
                var fileContent = File.ReadAllText(filePath)!;

                var words = fileContent.Split("\n")
                    .Select(word => word.Trim())
                    .Where(word => word != string.Empty);

                foreach (var word in words)
                {
                    CollinsWordFrequency frequency = new()
                    {
                        FrequencyLevel = level,
                        Spelling = word
                    };
                    db.Add(frequency);
                }
            }

            db.SaveChanges();
        }
    }
}
