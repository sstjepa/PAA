using System.Diagnostics;

namespace PIA_Zad2
{
    class Program
    {
        public Stopwatch stopWatch = new Stopwatch();

        static void Main(string[] args)
        {
            int n, k;

            int[] array100 = new int[100];
            int[] array1000 = new int[1000];
            int[] array10k = new int[10000];
            int[] array100k = new int[100000];
            int[] arrat1m = new int[1000000];
            int[] array10m = new int[10000000];
            Console.WriteLine("Hello, World!");
            FillArray(array100);
            PrintArray(array100);

            Console.WriteLine("\n///////////////////////////");
            BubbleSort(array100);
            PrintArray(array100);

            Console.WriteLine("\n///////////////////////////");
            HeapSort(array100);
            PrintArray(array100);

        }

        public static void FillArray(int[] niz)
        {
            Random random = new Random();
            for (int i = 0; i < niz.Length; i++)
            {
                niz[i] = random.Next(10000);
            }
        }
        public static void PrintArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i]+" ");
            }
        }
        public static void BubbleSort(int[] array)
        {
            Stopwatch stopWatch = new Stopwatch();
            long memoryBefore = GC.GetTotalMemory(true);
            stopWatch.Start();
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            stopWatch.Stop();
            long memoryAfter = GC.GetTotalMemory(true);
            double vreme = stopWatch.Elapsed.Ticks / (double)10000;
            long memory = memoryAfter - memoryBefore;
            Console.WriteLine("Memory used for BubbleSort: " + (memory.ToString()));
            Console.WriteLine("Time used for BubbleSort: " + (vreme) + "ms");
        }

        public static void HeapSort(int[] array)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            long memoryBefore = GC.GetTotalMemory(true);
            int n = array.Length;
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(array, n, i);
            }
            for (int i = n - 1; i > 0; i--)
            {
                int temp = array[0];
                array[0] = array[i];
                array[i] = temp;
                Heapify(array, i, 0);
            }
            stopWatch.Stop();
            long memoryAfter = GC.GetTotalMemory(true);
            double vreme = stopWatch.Elapsed.Ticks/(double)10000;
            long memory = memoryAfter - memoryBefore;

            Console.WriteLine("Memory used for HeapSort: " + (memory.ToString()));
            Console.WriteLine("Time used for HeapSort: " + (vreme)+"ms");
        } 

        public static void Heapify(int[] array, int size, int index)
        {
            int largestIndex = index;
            int l = 2 * index + 1;
            int r = 2 * index + 2;
            if (l < size && array[l] > array[largestIndex])
            {
                largestIndex = l;
            }
            if (r < size && array[r] > array[largestIndex])
            {
                largestIndex = r;
            }
            if (largestIndex != index)
            {
                int swap = array[index];
                array[index] = array[largestIndex];
                array[largestIndex] = swap;
                Heapify(array, size, largestIndex);
            }
        }


    }
}
