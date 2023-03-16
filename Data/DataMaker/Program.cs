using WordBucket.Models;

namespace WordBucket.DataMaker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dictionary = new EnglishChineseDictionary(Id: 1, Name: "勇气");
            Console.Write(dictionary);
        }
    }
}
