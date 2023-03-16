namespace WordBucket.Models
{
    public record class LearningWord
    {
        public int? Id { get; set; }

        public string Spelling { get; set; } = string.Empty;

        public LearningProgress Progress { get; set; } = LearningProgress.None;
    }

    public enum LearningProgress
    {
        Ignored = -1,
        None = 0,
        Known = 1,
        Familiar = 2,
        Mastered = 3,
    }
}
