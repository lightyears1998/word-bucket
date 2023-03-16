namespace WordBucket.Models
{
    public record class WordFrequency
    {
        public int? Id { set; get; }

        public string Spelling { set; get; } = string.Empty;

        public int FrequencyLevel { set; get; }
    }
}
