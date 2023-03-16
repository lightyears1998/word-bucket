namespace WordBucket.Models
{
    public record class Corpus
    {
        public int Id { set; get; }

        public string Text { set; get; } = string.Empty;
    }
}
