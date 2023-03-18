using System.Data.Common;
using WordBucket.Contexts;
using WordBucket.Models;
using WordBucket.Services;

namespace WordBucket.DataMaker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DictionaryContext db = new("./dictionary.sqlite3");
            var dictionary = new EnglishChineseDictionary { Name = "C" };
            db.Add(dictionary);
            db.SaveChanges();
        }
    }
}
