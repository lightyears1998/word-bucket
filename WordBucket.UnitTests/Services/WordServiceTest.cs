using WordBucket.Services;

namespace WordBucket.UnitTests.Services
{
    public class WordServiceTest
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

        [Fact]
        public void f()
        {
            var path = AppConfig.DefaultUserDataDbPath;
        }
    }
}
