using System.Diagnostics;

namespace PIA_Zad2
{
    class Program
    {
        public Stopwatch stopWatch = new Stopwatch();

        static void Main(string[] args)
        {
            int[] array100 = new int[100];
            int[] array1000 = new int[1000];
            int[] array10k = new int[10000];
            int[] array100k = new int[100000];
            int[] array1m = new int[1000000];
            int[] array10m = new int[10000000];

            int[][] ints = new int[][] { array100, array1000, array10k, array100k, array1m, array10m };
            foreach (int[] array in ints)
            {
                FillArray(array);
            }

            Problem(ints);
        }

        public static void Problem(int[][] ints)
        {
            Console.WriteLine("Izaberite jednu opciju: \n1) Bubble Sort \n2) Heap Sort \n3) Radix Sort");
            int? opcija = null;
            bool isValid = false;

            while (!isValid)
            {
                opcija = Convert.ToInt32(Console.ReadLine());
                if (opcija == 1 || opcija == 2 || opcija == 3)
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("Unesite validnu opciju!");
                }
            }


            foreach (int[] array in ints)
            {
                int n = array.Length;
                int k = n*2/10;
                if (opcija == 1) 
                {
                    BubbleSort(array);
                }
                else if (opcija == 2)
                {
                    HeapSort(array);
                }
                else
                {
                    RadixSort(array);
                }

                int cost = 0;
                int right = n - 1;

                for (int i = 0; i <= right; i++)
                {
                    cost += array[i];
                    right -= k;
                }
                Console.WriteLine("Cheapest cost: " + cost);
            }
        }

        public static void FillArray(int[] array)
        {
            Random random = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(1,10000);
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
            double vreme = stopWatch.Elapsed.TotalMilliseconds;
            long memoryUsage = memoryAfter - memoryBefore;
            Console.WriteLine("Memory used for BubbleSort: " + memoryUsage.ToString());
            Console.WriteLine("Time used for BubbleSort: " + vreme + "ms");
        }

        public static void HeapSort(int[] array)
        {
            Stopwatch stopWatch = new Stopwatch();
            long memoryBefore = GC.GetTotalMemory(true);
            stopWatch.Start();
            
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
            double vreme = stopWatch.Elapsed.TotalMilliseconds;
            long memoryUsed = memoryAfter - memoryBefore;

            Console.WriteLine("Memory used for HeapSort: " + memoryUsed.ToString());
            Console.WriteLine("Time used for HeapSort: " + vreme +"ms");
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

        public static void RadixSort(int[] array)
        {
            Stopwatch stopWatch = new Stopwatch();
            long memoryBefore = GC.GetTotalMemory(true);
            stopWatch.Start();
            int n = array.Length;
            int max = array.Max();
            for (int exp = 1; max / exp > 0; exp *= 10)
            {
                CountingSort(array, n, exp);
            }
            stopWatch.Stop();
            long memoryAfter = GC.GetTotalMemory(true);
            double vreme = stopWatch.Elapsed.TotalMilliseconds;
            long memoryUsed = memoryAfter - memoryBefore;

            Console.WriteLine("Memory used for RadixSort: " + memoryUsed.ToString());
            Console.WriteLine("Time used for RadixSort: " + vreme + "ms");
        }

        public static void CountingSort(int[] array, int size, int exp)
        {
            int[] output = new int[size];
            int[] count = new int[10];

            for (int i = 0; i < 10; i++)
            {
                count[i] = 0;
            }
            for (int i = 0; i < size; i++)
            {
                count[(array[i] / exp) % 10]++;
            }
            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }
            for (int i = size - 1; i >= 0; i--)
            {
                output[count[(array[i] / exp) % 10] - 1] = array[i];
                count[(array[i] / exp) % 10]--;
            }
            for (int i = 0; i < size; i++)
            {
                array[i] = output[i];
            }
        }
    }
}
