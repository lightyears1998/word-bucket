using WordBucket.Contexts;

namespace WordBucket.UnitTests.Contexts
{
    public class ContextTests
    {
        [Fact]
        public void Test1()
        {
            var dictCtx = new DictionaryContext();
            var userCtx = new UserContext();

            dictCtx.Database.EnsureCreated();
            userCtx.Database.EnsureCreated();
        }
    }
}
