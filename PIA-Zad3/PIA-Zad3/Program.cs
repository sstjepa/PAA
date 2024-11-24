namespace PIA_Zad3
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int[] arr = new int[30];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = random.Next(-10, 10);
            }
            Console.WriteLine("Originalni niz: " + string.Join(", ", arr));
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                int start = random.Next(0, arr.Length);
                int n = random.Next(start + 1, arr.Length);

                int result = FindMinProduct(arr, start, n);
                Console.WriteLine($"Minimalni proizvod podniza od {start} do {n - 1}: {result}\n");
            }
        }

        static int FindMinProduct(int[] arr, int start, int n)
        {
            Console.Write("Podniz: ");
            for (int i = start; i < n; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.WriteLine();


            if (start == n)
                return arr[start];

            int negative = int.MinValue;
            int positive = int.MaxValue;
            int count_neg = 0, count_zero = 0;
            int mull = 1;

            for (int i = start; i < n; i++)
            {

                if (arr[i] == 0)
                {
                    count_zero++;
                    continue;
                }

                if (arr[i] < 0)
                {
                    count_neg++;
                    negative = Math.Max(negative, arr[i]);
                }

                if (arr[i] > 0 && arr[i] < positive)
                {
                    positive = arr[i];
                }

                mull *= arr[i];
            }

            if (count_neg == 0) 
            { 
                if (count_zero == n || count_zero>0)
                    return 0;
                else
                    return positive;
            }
                
            if (count_neg % 2 == 0 && count_neg != 0)
            {
                mull = mull / negative;
            }

            return mull;
        }
    }
}