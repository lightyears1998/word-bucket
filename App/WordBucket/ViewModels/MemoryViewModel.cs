using ReactiveUI;
using System.Linq;
using System.Windows.Input;
using WordBucket.Contexts;
using WordBucket.Models;

namespace WordBucket.ViewModels
{
    public class MemoryViewModel : ViewModelBase
    {
        private LearningWord? _word = null;

        private int _corpusIndex = 0;

        public string Spelling => _word?.Spelling ?? string.Empty;

        public string Definitions => _word?.Definitions ?? string.Empty;

        public bool IsUnfamiliar
        {
            get => _word != null && _word.Progress == LearningProgress.Unfamiliar;
            set
            {
                if (_word != null)
                {
                    _word.Progress = LearningProgress.Unfamiliar;
                }
            }
        }

        public bool IsFamiliar
        {
            get => _word != null && _word.Progress == LearningProgress.Familiar;
            set
            {
                if (_word != null)
                {
                    _word.Progress = LearningProgress.Familiar;
                }
            }
        }

        public bool IsMastered
        {
            get => _word != null && _word.Progress == LearningProgress.Mastered;
            set
            {
                if (_word != null)
                {
                    _word.Progress = LearningProgress.Mastered;
                }
            }
        }

        public string CorpusIndicatorLabelText => $"{(_word != null && _word.Corpuses.Count > 0 ? _corpusIndex + 1 : 0)} / {_word?.Corpuses.Count ?? 0}";

        public string CorpusText { get; protected set; } = string.Empty;

        public string CorpusSource { get; protected set; } = string.Empty;

        public string CorpusUri { get; protected set; } = string.Empty;

        public ICommand MoveOnToNextWordCommand { get; }

        public ICommand LoadNextCorpusCommand { get; }

        public MemoryViewModel()
        {
            MoveOnToNextWordCommand = ReactiveCommand.Create(MoveOnToNextWord);
            LoadNextCorpusCommand = ReactiveCommand.Create(LoadNextCorpus);
        }

        private void MoveOnToNextWord()
        {
            using var userContext = new UserContext();

            if (_word != null)
            {
                _word.LastVisit = DateTime.Now;
                userContext.SaveChanges();
            }

            LearningProgress[] filters = { LearningProgress.Unfamiliar, LearningProgress.Familiar };

            _word = userContext.LearningWords
                .Where(word => filters.Contains(word.Progress))
                .OrderBy(word => word.LastVisit)
                .FirstOrDefault();

            _corpusIndex = 0;
            this.RaisePropertyChanged(nameof(Spelling));
            this.RaisePropertyChanged(nameof(Definitions));
            this.RaisePropertyChanged(nameof(IsUnfamiliar));
            this.RaisePropertyChanged(nameof(IsFamiliar));
            this.RaisePropertyChanged(nameof(IsMastered));
            LoadNextCorpus();
        }

        private void LoadNextCorpus()
        {
            if (_word != null)
            {
                if (_word.Corpuses.Count != 0)
                {
                    _corpusIndex %= _word.Corpuses.Count;
                    var corpus = _word.Corpuses[_corpusIndex];

                    CorpusText = corpus.Text;
                    CorpusSource = corpus.Source;
                    CorpusUri = corpus.Uri;

                    this.RaisePropertyChanged(nameof(CorpusIndicatorLabelText));
                    this.RaisePropertyChanged(nameof(CorpusText));
                    this.RaisePropertyChanged(nameof(CorpusSource));
                    this.RaisePropertyChanged(nameof(CorpusUri));
                }
            }
        }
    }
}
