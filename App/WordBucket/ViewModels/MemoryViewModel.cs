using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System.Linq;
using System.Windows.Input;
using WordBucket.Contexts;
using WordBucket.Models;

namespace WordBucket.ViewModels
{
    public class MemoryViewModel : ViewModelBase
    {
        private UserContext _userContext = new();

        private LearningWord? _word = null;

        private int _corpusIndex = 0;

        public string Spelling => _word?.Spelling ?? string.Empty;

        public string Definitions => _word?.Definitions ?? string.Empty;

        private bool _isUnfamiliar;

        public bool IsUnfamiliar
        {
            get => _isUnfamiliar;
            set
            {
                if (this.RaiseAndSetIfChanged(ref _isUnfamiliar, value))
                {
                    if (_word != null)
                        _word.Progress = LearningProgress.Unfamiliar;
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
                    if (_word != null)
                        _word.Progress = LearningProgress.Familiar;
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
                    if (_word != null)
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
            if (_word != null)
            {
                _word.LastVisit = DateTime.Now;
                _userContext.SaveChanges();
            }

            LearningProgress[] filters = { LearningProgress.Unfamiliar, LearningProgress.Familiar };

            _word = _userContext.LearningWords
                .Include(word => word.Corpuses)
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
