using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiveWordFiveLetters
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\HFGF\\source\\repos\\FiveWordLetters_02.sln\\FiveWordLetters_02.sln\\assets\\words_not_perfect.txt";
            List<string> words = LoadWordsFromFile(filePath);

            words = words.Where(IsValidWord).Distinct().ToList();

            var combinations = FindValidCombinations(words);

            
            foreach (var combo in combinations)
            {
                Console.WriteLine(string.Join(", ", combo));
            }

            Console.WriteLine($"Antal kombinationer fundet: {combinations.Count}");
        }

        static List<string> LoadWordsFromFile(string filePath)
        {
            try
            {
                return File.ReadAllLines(filePath).Select(line => line.Trim().ToLower()).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Fejl ved indlæsning af fil: " + e.Message);
                return new List<string>();
            }
        }

        static bool IsValidWord(string word)
        {
            return word.Length == 5 && word.All(char.IsLetter) && word.Distinct().Count() == 5;
        }

        static List<List<string>> FindValidCombinations(List<string> words)
        {
            var validCombinations = new List<List<string>>();
            var alphabet = new HashSet<char>("abcdefghijklmnopqrstuvwxyz");

            foreach (var combination in GetCombinations(words, 5))
            {
                var combinedLetters = combination.SelectMany(word => word).ToHashSet();
                if (combinedLetters.Count == 25 && combinedLetters.SetEquals(alphabet))
                {
                    validCombinations.Add(combination);
                }
            }

            return validCombinations;
        }

        static IEnumerable<List<string>> GetCombinations(List<string> words, int length)
        {
            if (length == 1) return words.Select(w => new List<string> { w });

            return words.SelectMany((word, index) =>
                GetCombinations(words.Skip(index + 1).ToList(), length - 1)
                .Select(combo => new List<string> { word }.Concat(combo).ToList()));
        }
    }
}