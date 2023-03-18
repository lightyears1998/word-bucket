using System.Data;
using WordBucket.Contexts;
using WordBucket.Models;

namespace WordBucket.DataMaker
{
    public class Program
    {
        public static void Main()
        {
            DictionaryContext db = new();
            db.Database.EnsureCreated();

            ParseCollins(db);
        }

        private static void ParseCollins(DictionaryContext db)
        {
            Console.WriteLine("Base Dir:");
            var baseDir = Console.ReadLine()!.Trim('"');

            for (int level = 0; level <= 5; ++level)
            {
                var filePath = Path.Join(baseDir, $"collins_{level}.txt");
                var fileContent = File.ReadAllText(filePath)!;
                var words = fileContent.Split("\n").Select(word => word.Trim()).Where(word => word != string.Empty);
                foreach (var word in words)
                {
                    CollinsWordFrequency freq = new CollinsWordFrequency { FrequencyLevel = level, Spelling = word };
                    db.Add(freq);
                }
            }

            db.SaveChanges();
        }
    }
}
