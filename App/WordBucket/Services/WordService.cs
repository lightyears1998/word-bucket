using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WordBucket.Contexts;
using WordBucket.Models;

namespace WordBucket.Services
{
    public class WordService
    {
        private readonly UserContext _userContext;

        public WordService(UserContext userContext)
        {
            _userContext = userContext;
        }

        public static IEnumerable<string> TryLemmatize(string word)
        {
            HashSet<string> result = new() { word };

            // Nouns
            if (word.EndsWith("s"))
            {
                result.Add(RemoveSuffix(word, "s"));
            }

            if (word.EndsWith("es"))
            {
                result.Add(RemoveSuffix(word, "es"));
            }

            // Verbs
            if (word.EndsWith("ed"))
            {
                result.Add(RemoveSuffix(word, "ed"));
                result.Add(RemoveSuffix(word, "d"));
            }

            if (word.EndsWith("en"))
            {
                result.Add(RemoveSuffix(word, "en"));
                result.Add(RemoveSuffix(word, "n"));
            }

            if (word.EndsWith("ing"))
            {
                var verb = RemoveSuffix(word, "ing");

                if (verb.Length >= 2 && verb[^1] == verb[^2])
                {
                    result.Add(verb[..^1]);
                }
            }

            return result;
        }

        private static string RemoveSuffix(string word, string suffix)
        {
            return word[..^suffix.Length];
        }

        public LearningWord GetLearningWord(Models.DictionaryEntry entry)
        {
            return GetLearningWord(entry.Spelling);
        }

        public LearningWord GetLearningWord(string spelling)
        {
            var word = _userContext.LearningWords
                .Include(word => word.Corpuses)
                .FirstOrDefault(word => word.Spelling == spelling);

            if (word == null)
            {
                using DictionaryContext dictionaryContext = new();
                var entry = dictionaryContext.DictionaryEntries.FirstOrDefault(entry => entry.Spelling == spelling);

                word = new LearningWord()
                {
                    Spelling = spelling,
                    Definitions = entry?.Definitions.Replace("\\n", " ") ?? "",
                    Progress = GetDefaultLearningProgress(spelling)
                };
            }

            return word;
        }

        public static LearningProgress GetDefaultLearningProgress(string spelling)
        {
            using var dictionary = new DictionaryContext();
            var frequencyLevelEntry = dictionary.Collins.FirstOrDefault(entry => entry.Spelling == spelling);

            if (frequencyLevelEntry == null)
            {
                return LearningProgress.None;
            }

            var level = frequencyLevelEntry.FrequencyLevel;
            if (level >= 4)
            {
                return LearningProgress.Ignored;
            }
            if (level == 3)
            {
                return LearningProgress.Familiar;
            }

            return LearningProgress.None;
        }

        public static void SortByLearningProgress(List<LearningWord> words)
        {
            var getRank = (LearningProgress progress) =>
            {
                return progress switch
                {
                    LearningProgress.Ignored => 100,
                    LearningProgress.None => 0,
                    LearningProgress.Unfamiliar => 1,
                    LearningProgress.Familiar => 2,
                    LearningProgress.Mastered => 3,
                    _ => throw new NotImplementedException(),
                };
            };

            words.Sort((a, b) =>
            {
                return getRank(a.Progress) - getRank(b.Progress);
            });
        }
    }
}
