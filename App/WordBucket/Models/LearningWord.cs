namespace WordBucket.Models
{
    public record class LearningWord
    {
        public int Id { get; set; }

        public string Spelling { get; set; } = string.Empty;

        public LearningProgress Progress { get; set; } = LearningProgress.None;
    }

    public enum LearningProgress
    {
        Ignored = -1,
        None = 0,
        Encountered = 1,
        Known = 2,
        Familiar = 3,
        Mastered = 4,
    }
}
