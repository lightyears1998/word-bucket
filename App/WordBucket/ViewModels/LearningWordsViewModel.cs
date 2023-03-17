using WordBucket.Models;

namespace WordBucket.ViewModels
{
    public class LearningWordsViewModel : ViewModelBase
    {
        public static LearningProgressItem[] ProgressNames => new[]
        {
            new LearningProgressItem("陌生", LearningProgress.Unfamiliar),
            new LearningProgressItem("熟悉", LearningProgress.Familiar),
            new LearningProgressItem("掌握", LearningProgress.Mastered),
        };

        public class LearningProgressItem
        {
            public LearningProgress EnumValue { get; }

            public string Name { get; }

            public LearningProgressItem(string name, LearningProgress enumValue)
            {
                Name = name;
                EnumValue = enumValue;
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
