namespace WordBucket.Models
{
    public record class CollinsWordFrequency
    {
        public int Id { set; get; }

        public string Spelling { set; get; } = string.Empty;

        public int FrequencyLevel { set; get; }
    }
}
