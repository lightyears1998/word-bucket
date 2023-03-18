using System.Collections.Generic;

namespace WordBucket.Models
{
    public record class Dictionary
    {
        public int Id { set; get; }

        public string Name { set; get; } = string.Empty;

        public List<DictionaryEntry> Entries { set; get; } = new();
    }
}
