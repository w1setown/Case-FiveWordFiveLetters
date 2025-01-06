using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FiveWordFiveLetters
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\HFGF\\source\\repos\\FiveWordLetters_02.sln\\FiveWordLetters_02.sln\\assets\\words_not_perfect.txt";
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<string> words = LoadWordsFromFile(filePath);
            if (words.Count == 0)
            {
                Console.WriteLine("Ingen ord blev indlæst. Kontrollér filens sti og format.");
                return;
            }

            // Filtrer gyldige ord
            words = words.Where(IsValidWord).Distinct().ToList();

            // Find kombinationer
            var combinations = FindValidCombinations(words);
            stopwatch.Stop();

            // Udskriv kombinationer
            if (combinations.Count > 0)
            {
                foreach (var combo in combinations)
                {
                    Console.WriteLine(string.Join(", ", combo));
                }
            }
            else
            {
                Console.WriteLine("Ingen gyldige kombinationer fundet.");
            }

            Console.WriteLine($"Antal kombinationer fundet: {combinations.Count}");
            Console.WriteLine($"Tid taget: {stopwatch.Elapsed.TotalSeconds:F2} sekunder");
        }

        static List<string> LoadWordsFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Filen blev ikke fundet: {filePath}");
                    return new List<string>();
                }

                return File.ReadAllLines(filePath)
                    .Select(line => line.Trim().ToLower())
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fejl ved indlæsning af fil: {e.Message}");
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

            foreach (var combination in GetCombinations(words, 5))
            {
                var combinedLetters = combination.SelectMany(word => word).ToHashSet();
                if (combinedLetters.Count == 25)
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
