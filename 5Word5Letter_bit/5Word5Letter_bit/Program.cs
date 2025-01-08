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
            string filePath = "C:\\Users\\HFGF\\source\\repos\\5Word5Letter_bit\\5Word5Letter_bit\\beta_words.txt";


            // Tid (ikke relevant for selve programmet)
            static void ShowElapsedTime(Stopwatch stopwatch, CancellationToken token)
            {
                while (!token.IsCancellationRequested)
                {
                    Console.Write($"\rTid forløbet: {stopwatch.Elapsed.TotalSeconds:F2} sekunder");
                    Thread.Sleep(100); // Opdater: 100 ms
                }
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            var cts = new CancellationTokenSource();
            Task.Run(() => ShowElapsedTime(stopwatch, cts.Token));





            List<string> words = LoadWordsFromFile(filePath);
            if (words.Count == 0)
            {
                Console.WriteLine("Ingen ord blev indlæst. Kontrollér filens sti og format.");
                cts.Cancel(); // Tid (ikke relevant for selve programmet)
                return;
            }

            // Filtrer gyldige ord og konverter til bitrepræsentation
            var bitWords = words.Where(IsValidWord)
                                .Distinct()
                                .Select(word => (Word: word, Bits: ConvertWordToBits(word)))
                                .ToList();


            // Find kombinationer
            var combinations = FindValidCombinations(bitWords); // virker ikke Argument 1: cannot convert from 'System.Collections.Generic.List<<anonymous type: string Word, int Bits>>' to 'System.Collections.Generic.List<(string Word, int Bits)>'
            stopwatch.Stop();
            cts.Cancel(); // Tid (ikke relevant for selve programmet)


            // Udskriv kombinationer
            if (combinations.Count > 0)
            {
                foreach (var combo in combinations)
                {
                    Console.WriteLine(string.Join(", ", combo.Select(x => x.Word)));
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


        static bool IsValidWord(string word) // To bool or not to bool is the question - William Booleanspear
        {
            return word.Length == 5 && word.All(char.IsLetter) && word.Distinct().Count() == 5;
        }


        static int ConvertWordToBits(string word) 
        {
            int bit = 0;
            foreach (var ch in word)
            {
                bit |= 1 << (ch - 'a');
            }
            return bit;
        }


        static List<List<(string Word, int Bits)>> FindValidCombinations(List<(string Word, int Bits)> words)
        {
            var validCombinations = new List<List<(string Word, int Bits)>>();

            foreach (var word in words)
            {
                FindCombinationsRecursive(words, new List<(string, int)> { word }, ~word.Bits, validCombinations);
            }

            return validCombinations;
        }


        static void FindCombinationsRecursive(List<(string Word, int Bits)> words, List<(string Word, int Bits)> currentCombination, int availableBits, List<List<(string Word, int Bits)>> validCombinations)
        {
            // Hvis vi har 5 ord, tjek om 25 bogstaver er brugt og 1 bogstav er i overskud
            if (currentCombination.Count == 5)
            {
                int usedBits = ~availableBits & ((1 << 26) - 1); // Brugte bogstaver
                int unusedBits = availableBits & ((1 << 26) - 1); // Ubrugte bogstaver

                if (CountBits(usedBits) == 25 && CountBits(unusedBits) == 1) // 1 overskud af ord
                {
                    validCombinations.Add(new List<(string Word, int Bits)>(currentCombination));
                }
                return;
            }

            foreach (var word in words)
            {
                if (currentCombination.Contains(word)) continue;

                if ((word.Bits & availableBits) == word.Bits) // Kan ordet bruges?????????
                {
                    currentCombination.Add(word);
                    FindCombinationsRecursive(words, currentCombination, availableBits & ~word.Bits, validCombinations);
                    currentCombination.RemoveAt(currentCombination.Count - 1);
                }
            }
        }


        static int CountBits(int n)
        {
            int count = 0;
            while (n != 0)
            {
                count += n & 1;
                n >>= 1;
            }
            return count;
        }
    }
}