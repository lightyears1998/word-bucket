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

        private string _corpusSource = string.Empty;

        public string CorpusSource
        {
            get => _corpusSource;
            set => this.RaiseAndSetIfChanged(ref _corpusSource, value);
        }

        private string _corpusUri = string.Empty;

        public string CorpusUri
        {
            get => _corpusUri;
            set => this.RaiseAndSetIfChanged(ref _corpusUri, value);
        }

        public ICommand QueryCommand { get; }

        public ICommand CollectCommand { get; }

        public ObservableCollectionEx<LearningWordItem> LearningWordsItem { get; } = new();

        public CollectViewModel()
        {
            QueryCommand = ReactiveCommand.Create(SearchByText);
            CollectCommand = ReactiveCommand.Create(CollectWords);
        }

        public void SearchByText()
        {
            SearchText = SearchText.Trim();
            CorpusSource = CorpusSource.Trim();
            CorpusUri = CorpusUri.Trim();

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

            var words = dictionaryEntries.AsParallel().Select(WordService.GetLearningWord).ToList();
            WordService.SortByLearningProgress(words);

            LearningWordsItem.Reload(words.Select(word => new LearningWordItem(word)));
        }

        public void CollectWords()
        {
            using var userContext = new UserContext();
            var words = LearningWordsItem.Select(item => item.Word).Where(item => item.Progress != LearningProgress.None).ToList();

            if (words.Count > 0)
            {
                Corpus? corpus = userContext.Corpuses.FirstOrDefault(corpus => corpus.Text == SearchText);
                if (corpus == null)
                {
                    corpus = new Corpus()
                    {
                        Source = CorpusSource,
                        Uri = CorpusUri,
                        Text = SearchText
                    };
                    userContext.Corpuses.Add(corpus);
                }

                foreach (var word in words)
                {
                    if (word.Id == 0)
                        userContext.LearningWords.Add(word);

                    if (!word.Corpuses.Any(corpus => corpus.Text == SearchText))
                        word.Corpuses.Add(corpus);
                }

                userContext.SaveChanges();

                SearchText = string.Empty;
                LearningWordsItem.Clear();
            }
        }

        public class LearningWordItem : ReactiveObject
        {
            public LearningWord Word { get; set; }

            public string Spelling => Word.Spelling;

            public string Definitions => Word.Definitions;

            private bool _isIgnore;

            public bool IsIgnore
            {
                get => _isIgnore;
                set
                {
                    if (this.RaiseAndSetIfChanged(ref _isIgnore, value))
                    {
                        Word.Progress = LearningProgress.Ignored;
                    }
                }
            }

            private bool _isNone;

            public bool IsNone
            {
                get => _isNone;
                set
                {
                    if (this.RaiseAndSetIfChanged(ref _isNone, value))
                    {
                        Word.Progress = LearningProgress.None;
                    }
                }
            }

            private bool _isUnfamiliar;

            public bool IsUnfamiliar
            {
                get => _isUnfamiliar;
                set
                {
                    if (this.RaiseAndSetIfChanged(ref _isUnfamiliar, value))
                    {
                        Word.Progress = LearningProgress.Unfamiliar;
                    }
                }
            }

            private bool _isFamiliar;

            public bool IsFamiliar
            {
                get => _isFamiliar;
                set
                {
                    if (this.RaiseAndSetIfChanged(ref _isFamiliar, value))
                    {
                        Word.Progress = LearningProgress.Familiar;
                    }
                }
            }

            private bool _isMastered;

            public bool IsMastered
            {
                get => _isMastered;
                set
                {
                    if (this.RaiseAndSetIfChanged(ref _isMastered, value))
                    {
                        Word.Progress = LearningProgress.Mastered;
                    }
                }
            }

            public LearningWordItem(LearningWord word)
            {
                Word = word;
                IsIgnore = Word.Progress == LearningProgress.Ignored;
                IsNone = Word.Progress == LearningProgress.None;
                IsUnfamiliar = Word.Progress == LearningProgress.Unfamiliar;
                IsFamiliar = Word.Progress == LearningProgress.Familiar;
                IsMastered = Word.Progress == LearningProgress.Mastered;
            }
        }
    }
}
