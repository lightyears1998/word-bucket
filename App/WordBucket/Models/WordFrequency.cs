using SQLite;

namespace WordBucket.Models
{
    public class WordFrequency
    {
        [PrimaryKey]
        public int? Id { set; get; }

        public string Spelling { set; get; } = string.Empty;

        public int FrequencyLevel { set; get; }
    }
}
