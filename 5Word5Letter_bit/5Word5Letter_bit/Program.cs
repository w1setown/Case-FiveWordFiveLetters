using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FiveWordFiveLetters_BitShift
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "C:\\Users\\HFGF\\source\\repos\\5Word5Letter_bit\\5Word5Letter_bit\\unique_words.txt";
            int wordLength = 5; // Længden af hvert ord
            int requiredWordCount = 5; // Antallet af ord i en kombination

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Indlæs og filtrer i filen 
            IDictionary<int, string> bitWords = LoadWords(filePath, wordLength);

            // Tjek efter gyldige ord
            if (bitWords.Count == 0)
            {
                Console.WriteLine("Der blev ikke fundet nogen gyldige ord.");
                return;
            }

            Console.WriteLine($"Der blev indlæst {bitWords.Count} unikke ord fra filen."); // Vis antal unikke ord

            // Find gyldige kombinationer af ord
            int totalCombinations = 0;
            RecursiveFindCombinations(bitWords.Keys.ToArray(), 0, 0, bitWords.Count - 1, requiredWordCount, ref totalCombinations);

            // Stop måling af udførelsestid
            stopwatch.Stop();
            Console.WriteLine($"Der blev fundet {totalCombinations} gyldige kombinationer af {requiredWordCount} ord uden overlap.");
            Console.WriteLine($"Programmet tog {stopwatch.ElapsedMilliseconds} ms om at køre.");
            Console.WriteLine($"Programmet tog {stopwatch.ElapsedMilliseconds / 1000.0} sekunder om at køre.");
        }

        // Indlæser ord og filtrere dem baseret på længde/unikhed
        static IDictionary<int, string> LoadWords(string filePath, int wordLength)
        {
            var bitWords = new Dictionary<int, string>();

            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    string word = line.Trim().ToLower();

                    // Valider ord: skal være 5 bogstaver langt og indeholde unikke tegn
                    if (word.Length == wordLength && word.Distinct().Count() == wordLength)
                    {
                        int bits = WordToBits(word); // Konverter ord til bit-repræsentation
                        if (!bitWords.ContainsKey(bits)) // Undgå dubletter
                        {
                            bitWords.Add(bits, word); // Tilføj gyldigt ord til ordbog
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fejl under indlæsning af filen: {e.Message}");
            }

            return bitWords;
        }

        // Bitshift finder sted
        static int WordToBits(string word)
        {
            int bits = 0;
            foreach (char c in word)
            {
                bits |= 1 << (c - 'a'); // Sætter bit til det specifikke tegn
            }
            return bits; // Returnér bit'en
        }

        // Rekursiv til at finde kombinationer af ord uden overlappende bogstaver
        static void RecursiveFindCombinations(int[] bitWords, int usedBits, int wordCount, int index, int requiredWordCount, ref int totalCombinations)
        {
            for (int i = index; i >= 0; i--)
            {
                // Sørg for, at der ikke er overlap af bogstaver
                if ((usedBits & bitWords[i]) == 0)
                {
                    // Hvis det krævede antal ord er nået inkrement total kombinationer
                    if (wordCount == requiredWordCount - 1)
                    {
                        totalCombinations++;
                        continue; 
                    }
                    // Rekursiv: find kombinationer med det nuværende ord inkluderet
                    RecursiveFindCombinations(bitWords, usedBits | bitWords[i], wordCount + 1, i - 1, requiredWordCount, ref totalCombinations);
                }
            }
        }
    }
}