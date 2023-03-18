using Microsoft.EntityFrameworkCore;
using WordBucket.Models;

namespace WordBucket.Contexts
{
    public class DictionaryContext : DbContext
    {
        public string DbPath { get; set; }

        public DbSet<Dictionary> Dictionaries { set; get; }

        public DbSet<DictionaryEntry> DictionaryEntries { set; get; }

        public DbSet<CollinsWordFrequency> Collins { set; get; }

        public DictionaryContext(string? dbPath = null)
        {
            DbPath = dbPath ?? AppConfig.DefaultDictionaryDataDbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}
