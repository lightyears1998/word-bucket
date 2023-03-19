using WordBucket.Contexts;

namespace WordBucket.UnitTests.Contexts
{
    public class ContextTests
    {
        [Fact]
        public void Test1()
        {
            using var dictionaryCtx = new DictionaryContext();
            using var userCtx = new UserContext();

            dictionaryCtx.Database.EnsureCreated();
            userCtx.Database.EnsureCreated();
        }
    }
}
