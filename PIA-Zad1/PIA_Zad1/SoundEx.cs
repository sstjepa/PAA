using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace PIA_Zad1
{
    class SoundEX
    {
        public static string SoundEx(string word)
        {
            word = word.ToUpper();
            char firstLetter = word[0];
            char? prevcode = null;

            string soundexCode = firstLetter.ToString();
            Dictionary<char, char> mapping = new Dictionary<char, char>()
            {
                { 'B', '1' }, { 'F', '1' }, { 'P', '1' }, { 'V', '1' },
                { 'C', '2' }, { 'G', '2' }, { 'J', '2' }, { 'K', '2' }, { 'Q', '2' }, { 'S', '2' }, { 'X', '2' }, { 'Z', '2' },
                { 'D', '3' }, { 'T', '3' },
                { 'L', '4' },
                { 'M', '5' }, { 'N', '5' },
                { 'R', '6' }
            };
            Dictionary<char, char> vowels = new Dictionary<char, char>()
            {
                { 'A', '0' }, { 'E', '0' }, { 'I', '0' },{ 'O', '0' },{ 'U', '0' }
            };

            for (int i = 1; i < word.Length; i++)
            {
                if (mapping.ContainsKey(word[i]))
                {
                    char code = mapping[word[i]];
                    if (mapping.ContainsKey(word[i-1]))
                    {
                        if (i == 1 && code == mapping[firstLetter])
                        {
                            prevcode = code;
                            continue;
                        }
                    }
                    if (code == prevcode && !vowels.ContainsKey(word[i - 1]))
                    {
                        if (word[i - 1] == 'H' || word[i - 1] == 'W') 
                        {
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    soundexCode += code;
                    prevcode = code;
                }
                else
                {
                    if (vowels.ContainsKey(word[i])) 
                    {
                        prevcode = null; 
                    }
                }
            }

            soundexCode = soundexCode.PadRight(4, '0').Substring(0, 4);
            return soundexCode;
        }

        public static List<string> FindWordsWithSameSoundExCode(string filePath, string inputWord)
        {
            string targetCode = SoundEx(inputWord);
            
            List<string> matchingWords = new List<string>();

            string[] words = File.ReadAllText(filePath).Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            

            foreach (string word in words)
            {
                if (word.ToUpper() != inputWord.ToUpper() && SoundEx(word) == targetCode)
                {
                    matchingWords.Add(word);
                }
            }

            return matchingWords;
        }

        public static void Call()
        {
            File.WriteAllText("../../../resultSound.txt", string.Empty);

            string[] filePaths = { @"../../../star_wars_100.txt", "../../../star_wars_1000.txt", "../../../star_wars_10000.txt", "../../../star_wars_100000.txt" };

            for (int i = 0; i < 3; i++)
            {
                Console.Write("Unesite rec za trazenje pogresno napisanih: ");
                string? inputWord = Console.ReadLine();
                while (string.IsNullOrEmpty(inputWord))
                {
                    Console.Write("Unos ne moze biti prazan!!!\nUnesite rec za trazenje pogresno napisanih: ");
                    inputWord = Console.ReadLine();
                }

                string targetCode = SoundEx(inputWord);

                foreach (string filePath in filePaths)
                {
                    Stopwatch stopwatch = new Stopwatch();

                    stopwatch.Start();
                    List<string> result = FindWordsWithSameSoundExCode(filePath, inputWord);
                    stopwatch.Stop();

                    double time = stopwatch.ElapsedTicks/ (double)10000;
                    Console.WriteLine($"\nFajl: {filePath.Remove(0, 9).ToString()}");
                    Console.WriteLine($"Vreme izvrsenja: {time} ms");
                    Console.WriteLine($"Pronadjene reci sa istim SoundEx kodom ({targetCode}):");
                    

                    using (StreamWriter of = new StreamWriter(@"../../../resultSound.txt", true))
                    {
                        
                        of.WriteLine($"\n{i}-{filePath.Remove(0,9).ToString()}-Prosečno vreme: {time} ms\n");
                        of.WriteLine($"SoundEx kod za unetu rec {inputWord}: ({targetCode})");

                        foreach (string word in result)
                        {
                            Console.WriteLine(word);
                            of.Write(word + " ");
                        }

                    }
                }
            }
        }
    }

}
