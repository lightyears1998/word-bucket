using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WordBucket.Contexts;
using WordBucket.Models;
using WordBucket.Services;

namespace WordBucket.ViewModels
{
    public class CollectViewModel : ViewModelBase
    {
        private string _searchText = string.Empty;

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public ICommand QueryCommand { get; }

        public ICommand CollectCommand { get; }

        public ObservableCollectionEx<LearningWordItem> LearningWords { get; } = new();

        public CollectViewModel()
        {
            QueryCommand = ReactiveCommand.Create(SearchByText);

            CollectCommand = ReactiveCommand.Create(() => { });
        }

        public void SearchByText()
        {
            SearchText = SearchText.Trim();

            var inputWords = SearchText.Split(" ");
            HashSet<string> candidateWords = new();

            foreach (var candidate in inputWords.AsParallel().Select(WordService.TryLemmatize))
            {
                candidateWords.UnionWith(candidate);
            }

            var matches = candidateWords.AsParallel().Select(word =>
            {
                using DictionaryContext dictionaryContext = new();
                var match = dictionaryContext.DictionaryEntries.Where(entry => entry.Spelling == word);
                return match.ToHashSet();
            }).ToList();

            HashSet<DictionaryEntry> dictionaryEntries = new();
            foreach (var match in matches)
            {
                dictionaryEntries.UnionWith(match);
            }

            var words = dictionaryEntries.AsParallel().Select(WordService.GetLearningWord);
            LearningWords.Reload(words.Select(word => new LearningWordItem(word)));
        }

        public class LearningWordItem
        {
            public LearningWord Word { get; set; }

            public string Spelling => Word.Spelling;

            public string Definitions => Word.Definitions;

            public bool IsIgnore
            {
                get => Word.Progress == LearningProgress.Ignored;
                set => Word.Progress = LearningProgress.Ignored;
            }

            public bool IsNone
            {
                get => Word.Progress == LearningProgress.None;
                set => Word.Progress = LearningProgress.None;
            }

            public bool IsUnfamiliar
            {
                get => Word.Progress == LearningProgress.Unfamiliar;
                set => Word.Progress = LearningProgress.Unfamiliar;
            }

            public bool IsFamiliar
            {
                get => Word.Progress == LearningProgress.Familiar;
                set => Word.Progress = LearningProgress.Familiar;
            }

            public bool IsMastered
            {
                get => Word.Progress == LearningProgress.Mastered;
                set => Word.Progress = LearningProgress.Mastered;
            }

            public LearningWordItem(LearningWord word)
            {
                Word = word;
            }
        }
    }
}
