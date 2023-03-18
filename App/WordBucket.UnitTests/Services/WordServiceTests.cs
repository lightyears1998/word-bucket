using WordBucket.Services;

namespace WordBucket.UnitTests.Services
{
    public class WordServiceTests
    {
        [Theory]
        [InlineData("shopping", "shop")]
        [InlineData("cats", "cat")]
        [InlineData("potatoes", "potato")]
        [InlineData("canceled", "cancel")]
        public void TestTryLemmatize(string word, string lemma)
        {
            var result = WordService.TryLemmatize(word);
            Assert.Contains(lemma, result);
        }
    }
}
