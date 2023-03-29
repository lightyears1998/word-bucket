using Microsoft.EntityFrameworkCore;
using WordBucket.Models;

namespace WordBucket.Contexts
{
    public class UserContext : DbContext
    {
        public string DbPath { get; set; }

        public DbSet<LearningWord> LearningWords { set; get; }

        public DbSet<Corpus> Corpuses { set; get; }

        public UserContext(string? dbPath = null)
        {
            DbPath = dbPath ?? UserSettings.Current.UserDataDbPath ?? AppConfig.DefaultUserDataDbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
