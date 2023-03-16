namespace WordBucket.Models
{
    public record class Dictionary
    {
        public int Id { set; get; }

        public string Name { set; get; } = string.Empty;
    }
}
