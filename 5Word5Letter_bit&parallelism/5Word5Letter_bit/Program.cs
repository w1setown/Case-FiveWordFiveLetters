using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace FiveWordFiveLetters_BitShift
{
    internal class Program
    {
        // Hyppighed-alfabet
        static readonly char[] FrequencyOrderedAlphabet =
            "zqxjkvbpgfywmucldrhsnioate".ToCharArray();

        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\Gabriel\\Desktop\\5Word5Letter_bit\\5Word5Letter_bit\\unique_words.txt";
            int wordLength = 5, requiredWordCount = 5;

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Indlæs og filtrer i filen
            var bitWords = LoadWords(filePath, wordLength);

            // Tjek efter gyldige ord
            if (bitWords.Count == 0)
            {
                Console.WriteLine("No valid words found.");
                return;
            }

            Console.WriteLine($"Loaded {bitWords.Count} unique words.");

            // Ord sorterés baseret på hyppighed via bits
            var sortedBitWords = bitWords.Keys.OrderBy(CountBits).ToArray();

            int totalCombinations = 0;

            // Find gyldige kombinationer vha. parallelism
            Parallel.ForEach(sortedBitWords, usedBits =>
            {
                int localTotal = 0;
                RecursiveFindCombinations(sortedBitWords, usedBits, 1, Array.IndexOf(sortedBitWords, usedBits) - 1, requiredWordCount, ref localTotal);
                Interlocked.Add(ref totalCombinations, localTotal);
            });

            // Tid og konsol output
            stopwatch.Stop();
            Console.WriteLine($"Found {totalCombinations} valid combinations of {requiredWordCount} words.");
            Console.WriteLine($"Program took {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
        }

        // Indlæser ord og filtrer dem
        static IDictionary<int, string> LoadWords(string filePath, int wordLength)
        {
            var bitWords = new Dictionary<int, string>();
            foreach (var word in File.ReadLines(filePath).Select(line => line.Trim().ToLower()))
            {
                if (word.Length == wordLength && word.Distinct().Count() == wordLength)
                {
                    int bits = word.Aggregate(0, (acc, c) => acc | (1 << Array.IndexOf(FrequencyOrderedAlphabet, c)));
                    if (!bitWords.ContainsKey(bits)) bitWords[bits] = word;
                }
            }
            return bitWords;
        }

        // Tæller bits
        static int CountBits(int bits) => Convert.ToString(bits, 2).Count(b => b == '1');

        // Gyldige kombinationer vha parallellism
        static void RecursiveFindCombinations(int[] bitWords, int usedBits, int wordCount, int index, int requiredWordCount, ref int totalCombinations)
        {
            if (wordCount == requiredWordCount)
            {
                Interlocked.Increment(ref totalCombinations);
                return;
            }
            for (int i = index; i >= 0; i--)
                if ((usedBits & bitWords[i]) == 0)
                    RecursiveFindCombinations(bitWords, usedBits | bitWords[i], wordCount + 1, i - 1, requiredWordCount, ref totalCombinations);
        }
    }
}