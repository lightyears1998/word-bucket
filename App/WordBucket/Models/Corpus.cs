using SQLite;

namespace WordBucket.Models
{
    public class Corpus
    {
        [PrimaryKey]
        public int? Id { set; get; }

        public string Text { set; get; } = string.Empty;
    }
}
