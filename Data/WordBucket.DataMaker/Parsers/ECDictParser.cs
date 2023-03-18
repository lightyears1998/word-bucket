using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using WordBucket.Contexts;
using WordBucket.Models;

namespace WordBucket.DataMaker.Parsers
{
    internal static class ECDictParser
    {
        private static readonly string DictionaryName = "ECDict";

        public static void Parse(string path)
        {
            SetupDictionary();
            var data = ReadData(path);
            List<DictionaryEntry> entries = ConvertToEntries(data);
            WriteDatabase(entries);
        }

        private static void WriteDatabase(List<DictionaryEntry> entries)
        {
            DictionaryContext context = new DictionaryContext();

            for (int i = 0; i < entries.Count; i++)
            {
                if (i % 100 == 0)
                {
                    Console.WriteLine($"Saving: {i / 100} of {entries.Count / 100}.");
                    context.SaveChanges();
                    context.Dispose();
                    context = new DictionaryContext();
                }

                entries[i].Dictionary = context.Dictionaries.Where(dict => dict.Name == DictionaryName).First();
                context.Add(entries[i]);
            }

            if (context != null)
            {
                context.SaveChanges();
                context.Dispose();
            }
        }

        private static List<DictionaryEntry> ConvertToEntries(IEnumerable<ECDictEntry> data)
        {
            Console.WriteLine("Converting to entry...");

            var entries = data.AsParallel().Select(record =>
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
            return entries;
        }

        private static void SetupDictionary()
        {
            using var context = new DictionaryContext();

            if (!context.Dictionaries.Any(dict => dict.Name == DictionaryName))
            {
                Dictionary dict = new() { Name = DictionaryName };

                context.Add(dict);
                context.SaveChanges();
            }
        }

        private static IEnumerable<ECDictEntry> ReadData(string path)
        {
            Console.WriteLine("Reading data...");

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower()
            });
            var records = csv.GetRecords<ECDictEntry>().ToList();

            Console.WriteLine("Finished.");
            return records;
        }

        private record class ECDictEntry
        {
            public string Word { get; set; } = string.Empty;
            public string Phonetic { get; set; } = string.Empty;
            public string Translation { get; set; } = string.Empty;
        };
    }
}
