using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FiveWordFiveLetters_BitShift
{
    public partial class MainForm : Form
    {
        // Hyppighed-alfabet
        static readonly char[] FrequencyOrderedAlphabet =
            "zqxjkvbpgfywmucldrhsnioate".ToCharArray();

        private Button btnLoadAndProcess;
        private Label lblResult;
        private TextBox txtFilePath;

        public MainForm()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // File path input box
            txtFilePath = new TextBox { Left = 10, Top = 10, Width = 300 };
            Controls.Add(txtFilePath);

            // Button to load and process words
            btnLoadAndProcess = new Button { Text = "Load and Process", Left = 10, Top = 40, Width = 150 };
            btnLoadAndProcess.Click += BtnLoadAndProcess_Click;
            Controls.Add(btnLoadAndProcess);

            // Label for results
            lblResult = new Label { Left = 10, Top = 70, Width = 400, Height = 30 };
            Controls.Add(lblResult);
        }

        private void BtnLoadAndProcess_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text;
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Invalid file path.");
                return;
            }

            int wordLength = 5, requiredWordCount = 5;
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Indlæs og filtrer i filen
            var bitWords = LoadWords(filePath, wordLength);

            // Tjek efter gyldige ord
            if (bitWords.Count == 0)
            {
                lblResult.Text = "No valid words found.";
                return;
            }

            lblResult.Text = $"Loaded {bitWords.Count} unique words.";

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

            stopwatch.Stop();
            lblResult.Text = $"Found {totalCombinations} valid combinations of {requiredWordCount} words. Time: {stopwatch.Elapsed.TotalSeconds:F2} seconds.";
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

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
