using Avalonia;
using Avalonia.Platform;
using Microsoft.EntityFrameworkCore;
using System;
using WordBucket.Models;

namespace WordBucket.Contexts
{
    public class UserContext : DbContext
    {
        public string DbPath { get; set; }

        public string DefaultUserDataDbPath
        {
            get
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var platform = AvaloniaLocator.Current.GetService<IRuntimePlatform>()!;
                platform.GetRuntimeInfo()
            }
        }

        public DbSet<LearningWord> LearningWords { set; get; }

        public UserContext(string dbPath)
        {
            DbPath = dbPath;
        }
    }
}
