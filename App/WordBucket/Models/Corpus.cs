namespace WordBucket.Models
{
    public record class Corpus
    {
        public int Id { set; get; }

        public string Source { set; get; } = string.Empty;

        public string Uri { set; get; } = string.Empty;

        public string Text { set; get; } = string.Empty;

        public DateTime CreatedAt { set; get; } = DateTime.Now;
    }
}
