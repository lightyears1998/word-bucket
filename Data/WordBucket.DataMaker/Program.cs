using CsvHelper;
using CsvHelper.Configuration;
using System.Data;
using System.Globalization;
using WordBucket.Contexts;
using WordBucket.Models;

namespace WordBucket.DataMaker
{
    public class Program
    {
        private static DictionaryContext _db;
        private static string _path;

        static Program()
        {
            _db = new DictionaryContext();
            _db.Database.EnsureCreated();

            Console.WriteLine("Path:");
            _path = Console.ReadLine()!.Trim('"');
        }

        public static void Main()
        {
            ParseCollins();
            // ParseDictionary("ECDict");
        }

        private static void ParseCollins()
        {
            for (int level = 0; level <= 5; ++level)
            {
                var filePath = Path.Join(_path, $"collins_{level}.txt");
                var fileContent = File.ReadAllText(filePath)!;
                var words = fileContent.Split("\n").Select(word => word.Trim()).Where(word => word != string.Empty);
                foreach (var word in words)
                {
                    CollinsWordFrequency freq = new CollinsWordFrequency { FrequencyLevel = level, Spelling = word };
                    _db.Add(freq);
                }
            }

            _db.SaveChanges();
        }

        private static void ParseDictionary(string name)
        {
            Dictionary dict = new() { Name = name };
            _db.Add(dict);
            _db.SaveChanges();

            using var reader = new StreamReader(_path);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower()
            });
            var records = csv.GetRecords<ECDictEntry>();

            Console.WriteLine("Converting to entry...");

            var entries = records.AsParallel().Select(record =>
            {
                DictionaryEntry entry = new()
                {
                    Spelling = record.Word.Trim(),
                    PhoneticSymbols = record.Phonetic.Trim(),
                    Definitions = record.Translation.Trim()
                };
                return entry;
            }).ToList();

            Console.WriteLine("Converted to entry.");

            {
                DictionaryContext? context = null;

                try
                {
                    context = new DictionaryContext();

                    for (int i = 0; i < entries.Count; i++)
                    {
                        if (i % 100 == 0)
                        {
                            Console.WriteLine($"Saving: {i / 100} of {entries.Count / 100}.");
                            context.SaveChanges();
                            context.Dispose();
                            context = new DictionaryContext();
                        }

                        entries[i].Dictionary = context.Dictionaries.Where(dict => dict.Name == name).First();
                        context.Add(entries[i]);
                    }
                }
                finally
                {
                    if (context != null)
                    {
                        context.SaveChanges();
                        context.Dispose();
                    }
                }
            }
        }

        private record class ECDictEntry
        {
            public string Word { get; set; } = string.Empty;
            public string Phonetic { get; set; } = string.Empty;
            public string Translation { get; set; } = string.Empty;
        };
    }
}
