using System;
using System.IO;
using System.Linq;


namespace FiveWordFiveletters_01
{
    internal class readFile
    {
        [STAThread]
        static void Main(string[] args)
        {
            String line;
            try
            {
                StreamReader sr = new StreamReader("C:\\Users\\HFGF\\source\\repos\\FiveWordFiveletters_0.1\\FiveWordFiveletters_0.1\\assets\\words_perfect.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    Console.WriteLine(line);
                    line = sr.ReadLine();
                }
                sr.Close();
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exeption: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Jobs done");
            }
        }
    }
}
