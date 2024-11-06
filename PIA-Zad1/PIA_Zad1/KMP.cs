using System;
using System.Diagnostics;
using System.IO;

namespace PIA_Zad1
{
    class KMP
    {
        public static void KMPSearch(string pattern, string text)
        {
            int M = pattern.Length;
            int N = text.Length;

            int[] lps = CPF(pattern);

            int i = 0;
            int j = 0;
            while (i < N)
            {
                if (pattern[j] == text[i])
                {
                    i++;
                    j++;
                }

                if (j == M)
                {
                    j = lps[j - 1];
                }
                else if (i < N && pattern[j] != text[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i++;
                }
            }
        }

        private static int[] CPF(string pattern)
        {
            int M = pattern.Length;
            int[] lps = new int[M];
            int length = 0;

            int i = 1;
            while (i < M)
            {
                if (pattern[i] == pattern[length])
                {
                    length++;
                    lps[i] = length;
                    i++;
                }
                else
                {
                    if (length != 0)
                    {
                        length = lps[length - 1];
                    }
                    else
                    {
                        lps[i] = 0;
                        i++;
                    }
                }
            }

            return lps;
        }

        public static void Call()
        {
            File.WriteAllText("../../../resultKMP.txt", string.Empty);

            string[] filePaths = { @"../../../star_wars_100.txt", "../../../star_wars_1000.txt", "../../../star_wars_10000.txt", "../../../star_wars_100000.txt" };

            for (int i = 0; i < 4; i++)
            {
                Console.Write("Unesite podstring za pretragu: ");
                string? pattern = Console.ReadLine();
                while (string.IsNullOrEmpty(pattern))
                {
                    Console.Write("Unos ne moze biti prazan!!!\nUnesite podstring za pretragu: ");
                    pattern = Console.ReadLine();
                }

                foreach (string filePath in filePaths)
                {
                    double sum = 0;
                    for (int j = 0; j < 25; j++)
                    {
                        string text = File.ReadAllText(filePath);

                        Stopwatch stopwatch = new Stopwatch();

                        stopwatch.Start();
                        KMPSearch(pattern, text);
                        stopwatch.Stop();


                        double time = stopwatch.ElapsedTicks / (double)10000;
                        Console.WriteLine($"vreme: {time} ms");
                        sum += time;
                    }
                    using (StreamWriter of = new StreamWriter(@"../../../resultKMP.txt", true))
                    {
                        double avg = sum / 25;
                        of.WriteLine($"{i}-{filePath.Remove(0, 9).ToString()}-Prosečno vreme: {avg} ms\n");
                    }
                }
            }
        }
    }
}

