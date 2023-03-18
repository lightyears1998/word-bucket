using Microsoft.EntityFrameworkCore;

namespace WordBucket.Models
{
    [PrimaryKey(nameof(Spelling))]
    public record class CollinsWordFrequency
    {
        public string Spelling { set; get; } = string.Empty;

        public int FrequencyLevel { set; get; }
    }
}
