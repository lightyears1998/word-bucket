using WordBucket.Services;

namespace WordBucket.UnitTests.Services
{
    public class WordServiceTest
    {
        [Theory]
        [InlineData("shopping", "shop")]
        public void TestTryLemmatize(string word, string lemma)
        {
            var result = WordService.TryLemmatize(word);
            Assert.Contains(lemma, result);
        }
    }
}
