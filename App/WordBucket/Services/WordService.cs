﻿using System.Collections.Generic;
using System.Linq;
using WordBucket.Contexts;
using WordBucket.Models;

namespace WordBucket.Services
{
    public static class WordService
    {
        public static IEnumerable<string> TryLemmatize(string word)
        {
            HashSet<string> result = new() { word };

            // Nouns
            if (word.EndsWith("s"))
            {
                result.Add(word.RemoveSuffix("s"));
            }

            if (word.EndsWith("es"))
            {
                result.Add(word.RemoveSuffix("es"));
            }

            // Verbs
            if (word.EndsWith("ed"))
            {
                result.Add(word.RemoveSuffix("ed"));
                result.Add(word.RemoveSuffix("d"));
            }

            if (word.EndsWith("en"))
            {
                result.Add(word.RemoveSuffix("en"));
                result.Add(word.RemoveSuffix("n"));
            }

            if (word.EndsWith("ing"))
            {
                var verb = word.RemoveSuffix("ing");

                if (verb.Length >= 2 && verb[^1] == verb[^2])
                {
                    result.Add(verb[..^1]);
                }
            }

            return result;
        }

        private static string RemoveSuffix(this string word, string suffix)
        {
            return word[..^suffix.Length];
        }

        public static LearningWord GetLearningWord(Models.DictionaryEntry entry)
        {
            return GetLearningWord(entry.Spelling);
        }

        public static LearningWord GetLearningWord(string spelling)
        {
            using UserContext userContext = new UserContext();

            var word = userContext.LearningWords.FirstOrDefault(word => word.Spelling == spelling);
            if (word == null)
            {
                using DictionaryContext dictionaryContext = new DictionaryContext();
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
                return LearningProgress.Mastered;
            }
            if (level == 3)
            {
                return LearningProgress.Familiar;
            }

            return LearningProgress.None;
        }

        public static void SortByLearningProgress(List<LearningWord> words)
        {
            words.Sort((a, b) =>
            {
                return (int)((uint)a.Progress - (uint)b.Progress);
            });
        }
    }
}
