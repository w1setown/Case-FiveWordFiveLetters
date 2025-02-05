using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FiveWordFiveLetters_BitShift
{
    internal class Program
    {
        // Hyppighed-alfabet
        static readonly char[] FrequencyOrderedAlphabet = "zqxjkvbpgfywmucldrhsnioate".ToCharArray();

        // LetterFrequencePosition til bitmasker
        static readonly int[] LetterFrequencePosition = { 16, 9, 23, 25, 22, 10, 21, 5, 24, 1, 7, 12, 15, 6, 20, 3, 2, 11, 14, 19, 13, 17, 0, 8, 18, 4 };

        static void Main(string[] args)
        {
            string filePath = "unique_words.txt";
            int wordLength = 5, requiredWordCount = 5;

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Indlæs og filtrer ordene
            var bitWords = LoadWords(filePath, wordLength);

            // Tjek efter gyldige ord
            if (bitWords.Count == 0)
            {
                Console.WriteLine("No valid words found.");
                return;
            }

            Console.WriteLine($"Loaded {bitWords.Count} unique words.");

            // Parallelisering med ConcurrentBag til at samle resultater
            var solutionsBag = new ConcurrentBag<int>();
            int totalCombinations = 0;

            // Parallel søgning efter løsninger
            Parallel.ForEach(bitWords.Keys, usedBits =>
            {
                int localTotal = 0;
                RecursiveFindCombinations(bitWords.Keys.ToArray(), usedBits, 1, Array.IndexOf(bitWords.Keys.ToArray(), usedBits) - 1, requiredWordCount, ref localTotal);
                solutionsBag.Add(localTotal);
            });

            // Saml resultater fra ConcurrentBag
            totalCombinations = solutionsBag.Sum();

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
                    int bits = wordToInt(word);
                    if (!bitWords.ContainsKey(bits)) bitWords[bits] = word;
                }
            }
            return bitWords;
        }

        // Konverterer et ord til en bitmask
        static int wordToInt(string word)
        {
            int output = 0;
            foreach (char c in word)
            {
                var position = LetterFrequencePosition[c - 'a'];
                output |= 1 << position;
            }
            return output;
        }

        // Tæller bits
        static int CountBits(int bits) => Convert.ToString(bits, 2).Count(b => b == '1');

        // Rekursiv søgning efter kombinationer
        static void RecursiveFindCombinations(int[] bitWords, int usedBits, int wordCount, int index, int requiredWordCount, ref int totalCombinations)
        {
            if (wordCount == requiredWordCount)
            {
                Interlocked.Increment(ref totalCombinations);
                return;
            }
            for (int i = index; i >= 0; i--)
            {
                if ((usedBits & bitWords[i]) == 0)
                {
                    RecursiveFindCombinations(bitWords, usedBits | bitWords[i], wordCount + 1, i - 1, requiredWordCount, ref totalCombinations);
                }
            }
        }
    }
}