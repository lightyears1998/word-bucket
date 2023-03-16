using SQLite;

namespace WordBucket.Models
{
    public record class EnglishChineseDictionary
    {
        [PrimaryKey]
        public int? Id { set; get; }

        public string Name { set; get; } = string.Empty;
    }
}
