using SQLite;

namespace WordBucket.Models
{
    public record class DictionaryEntry
    {
        [PrimaryKey]
        public int? Id { get; set; }

        public string Spelling { get; set; } = string.Empty;

        public string Definitions { get; set; } = string.Empty;
    }
}
