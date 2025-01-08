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
            string filePath = "C:\\Users\\HFGF\\source\\repos\\5Words5Letters03\\5Words5Letters03\\assets\\beta_words.txt";
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

        // Tjek om filen bliver læst eller læst korrekt
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

        static bool IsValidWord(string word) // To bool or not to bool that is the question
        {
            return word.Length == 5 && word.All(char.IsLetter) && word.Distinct().Count() == 5;
        }

        static List<List<string>> FindValidCombinations(List<string> words)
        {
            var validCombinations = new List<List<string>>();
            var alphabet = new HashSet<char>("abcdefghijklmnopqrstuvwxyz"); // Hashsettet som bruges til at identificere

            foreach (var word in words)
            {
                // Kopierer alfabetet og fjerner bogstaver i det aktuelle ord
                var availableLetters = new HashSet<char>(alphabet);
                foreach (var letter in word)
                {
                    availableLetters.Remove(letter);
                }

                // Find kombinationer med de resterende bogstaver
                FindCombinationsRecursive(words, new List<string> { word }, availableLetters, validCombinations);
            }

            return validCombinations;
        }

        static void FindCombinationsRecursive(List<string> words, List<string> currentCombination, HashSet<char> availableLetters, List<List<string>> validCombinations)
        {
            // Stop hvis kombinationen er komplet
            if (currentCombination.Count == 5)
            {
                // Hvis der kun er ét bogstav tilbage accepter kombinationen
                if (availableLetters.Count <= 1)
                {
                    validCombinations.Add(new List<string>(currentCombination));
                }
                return;
            }

            foreach (var word in words)
            {
                if (currentCombination.Contains(word)) continue;

                // Tjek om ordet kan passe med de tilgængelige bogstaver
                if (word.All(availableLetters.Contains))
                {
                    // Fjern bogstaverne i det valgte ord
                    var updatedAvailableLetters = new HashSet<char>(availableLetters);
                    foreach (var letter in word)
                    {
                        updatedAvailableLetters.Remove(letter);
                    }

                    // Tilføj ordet til kombinationen + recursion
                    currentCombination.Add(word);
                    FindCombinationsRecursive(words, currentCombination, updatedAvailableLetters, validCombinations);
                    currentCombination.RemoveAt(currentCombination.Count - 1);
                }
            }
        }
    }
}