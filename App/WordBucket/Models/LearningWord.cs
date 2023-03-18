using System.Collections.Generic;

namespace WordBucket.Models
{
    public record class LearningWord
    {
        public int Id { get; set; }

        public string Spelling { get; set; } = string.Empty;

        public string Definitions { get; set; } = string.Empty;

        public LearningProgress Progress { get; set; } = LearningProgress.None;

        public DateTime LastVisit { get; set; }

        public List<Corpus> Corpuses { get; set; } = new();
    }

    public enum LearningProgress
    {
        Ignored = -1,
        None = 0,
        Unfamiliar = 1,
        Familiar = 2,
        Mastered = 3,
    }
}
