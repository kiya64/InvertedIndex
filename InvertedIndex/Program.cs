using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InvertedIndex
{
    public static class Extensions
    {

        public static IEnumerable<string> And(this Dictionary<string, List<string>> index, string firstTerm, string secondTerm)
        {
            return (from d in index
                    where d.Key.IndexOf(firstTerm, StringComparison.OrdinalIgnoreCase) >= 0
                    select d.Value).SelectMany(x => x).Intersect
                       ((from d in index
                         where d.Key.IndexOf(secondTerm, StringComparison.OrdinalIgnoreCase) >= 0
                         select d.Value).SelectMany(x => x));
        }


        public static IEnumerable<string> Or(this Dictionary<string, List<string>> index, string firstTerm, string secondTerm)
        {
            return (from d in index
                    where d.Key.IndexOf(firstTerm, StringComparison.OrdinalIgnoreCase) >= 0 || d.Key.IndexOf(secondTerm, StringComparison.OrdinalIgnoreCase) >= 0
                    select d.Value).SelectMany(x => x).Distinct();
        }
        public static IEnumerable<string> Not(this Dictionary<string, List<string>> index, string term)
        {
            return (from d in index
                    where d.Key.IndexOf(term, StringComparison.OrdinalIgnoreCase) == -1
                    select d.Value).SelectMany(x => x).Distinct();
        }
    }

    class EntryPoint
    {
        public static Dictionary<string, List<string>> invertedIndex;

        static void Main(string[] args)
        {
            invertedIndex = new Dictionary<string, List<string>>();
            string folder = "D:\\New folder\\";
            List<string> stopWords = new List<string> { "the", "and", "is" };

            foreach (string file in Directory.EnumerateFiles(folder, "*.txt"))
            {
                List<string> content = File.ReadAllText(file)
                    .Split(' ')
                    .Select(word => word.ToLower())
                    .Where(word => !stopWords.Contains(word))
                    .Distinct()
                    .ToList();

                addToIndex(content, file.Replace(folder, ""), stopWords);
            }
            string m = Console.ReadLine();
            string a = Console.ReadLine();

            var resAnd = invertedIndex.And(m, a);
            Console.WriteLine("Intersection Result (resAnd):");
            foreach (var item in resAnd)
            {
                Console.WriteLine(item);
            }
            var resOr = invertedIndex.Or(m, a);
            var resNot = invertedIndex.Not("Sherlock");
            Console.ReadLine();
        }


        private static void addToIndex(List<string> words, string document, List<string> stopWords)
        {
            foreach (var word in words)
            {
                if (!invertedIndex.ContainsKey(word))
                {
                    invertedIndex.Add(word, new List<string> { document });
                }
                else
                {
                    invertedIndex[word].Add(document);
                }
            }
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//class Program
//{
//    static void Main()
//    {





//        // نمونه ای کوچک
//        Dictionary<int, string> documents = new Dictionary<int, string>
//        {
//            { 1, "This is a sample document about programming" },
//            { 2, "C# is a powerful programming language" },
//            { 3, "Programming languages are essential for software development" },
//        };

//        Dictionary<string, List<int>> invertedIndex = BuildInvertedIndex(documents);

//        // نمایش ایندکس معکوس
//        Console.WriteLine("Inverted Index:");
//        foreach (var entry in invertedIndex)
//        {  
//            Console.WriteLine($"{entry.Key}: {string.Join(", ", entry.Value)}");
//        }

//        // جستجو
//        Console.WriteLine("\nEnter your query:");
//        string query = Console.ReadLine();
//        Console.WriteLine("1  AND\n2  OR\n3  NOT ");
//        int wch = int.Parse(Console.ReadLine());


//        List<int> result = SearchInvertedIndex(query, invertedIndex,wch);
//        // مناسب برای تعداد کم 
//        Console.WriteLine("\nSearch Result:");
//        foreach (var docId in result)
//        {
//            Console.WriteLine($"Document {docId}: {documents[docId]}");
//        }
//        Console.WriteLine("\nSearch Result:");


//        /// برای فایل های بزرگ
//        foreach (var docId in result)
//        {
//            Console.WriteLine($"Document {docId}");
//        }
//    }

//    static Dictionary<string, List<int>> BuildInvertedIndex(Dictionary<int, string> documents)
//    {
//        var invertedIndex = new Dictionary<string, List<int>>();

//        foreach (var document in documents)
//        {
//            int docId = document.Key;
//            string[] words = document.Value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);


//            foreach (var word in words)
//            {
//                string normalizedWord = NormalizeWord(word);

//                if (!invertedIndex.ContainsKey(normalizedWord))
//                {
//                    invertedIndex[normalizedWord] = new List<int>();
//                }

//                if (!invertedIndex[normalizedWord].Contains(docId))
//                {
//                    invertedIndex[normalizedWord].Add(docId);
//                }
//            }
//        }

//        return invertedIndex;
//    }

//    static List<int> SearchInvertedIndex(string query, Dictionary<string, List<int>> invertedIndex,int wch)
//    {
//        string[] terms = query.Split(' ');

//        List<int> result = new List<int>();

//        switch (wch)
//        {
//            case 1:
//                foreach (var term in terms)
//                {
//                    string normalizedTerm = NormalizeWord(term);
//                    if (result.Count == 0)
//                    {
//                        if (invertedIndex.ContainsKey(normalizedTerm))
//                        {

//                                result.AddRange(invertedIndex[normalizedTerm]);


//                        }
//                        else
//                        {
//                            result.Clear();
//                            break;
//                        }
//                    }
//                    else
//                    {
//                        string m = "";
//                        int i = 0;

//                        foreach (var item in result)
//                        {
//                            if (i == 0)
//                            {
//                                m=Convert.ToString(item);
//                            }
//                            else
//                            {
//                            m = m +" "+ Convert.ToString(item);
//                            }
//                            i++;
//                        }

//                        string a = "";
//                        foreach (var entry in invertedIndex)
//                        {
//                            if (term == entry.Key)
//                            {
//                                a = string.Join(" ", entry.Value);
//                            }
//                        }
//                        var x = m.Split(" ");
//                        var z = a.Split(" ");


//                        var commonElements = z.Intersect(x).ToArray();

//                        if (commonElements != null)
//                        {

//                            result.Clear();

//                            foreach (var element in commonElements)
//                            {
//                                result.Add(int.Parse(element));
//                            }


//                        }
//                        else
//                        {
//                            result.Clear();
//                            break;
//                        }

//                    }
//                }
//                break;

//            case 2:
//                {
//                    foreach (var term in terms)
//                    {
//                        string normalizedTerm = NormalizeWord(term);

//                        if (invertedIndex.ContainsKey(normalizedTerm))
//                        {
//                            result.AddRange(invertedIndex[normalizedTerm]);
//                        }
//                    }
//                    result = result.Distinct().ToList();


//                    break;

//                }
//            case 3:
//  foreach (var term in terms)
//    {
//        string normalizedTerm = NormalizeWord(term);

//        if (invertedIndex.ContainsKey(normalizedTerm))
//        {
//            result.AddRange(invertedIndex[normalizedTerm]);
//        }
//    }
//                List<int> allDocuments = new List<int>();
//                foreach (var entry in invertedIndex)
//                {
//                    allDocuments.AddRange(entry.Value);
//                }

//                result = result.Distinct().ToList();
//                List<int> notResult = allDocuments.Except(result).Distinct().ToList();
//                result.Clear();
//                result = notResult.ToList();
//                break;
//            default: break;
//        }

//        return result;
//    }
//    static string NormalizeWord(string word)
//    {
//        return word.ToLower();
//    }
//}
