namespace PIA_Zad4
{

    class Program
    {
        static bool IsMinHeap(List<int> heap)
        {
            for (int i = 0; i < heap.Count / 2; i++)
            {
                int leftChild = 2 * i + 1;
                int rightChild = 2 * i + 2;

                if (leftChild < heap.Count && heap[i] > heap[leftChild])
                    return false;

                if (rightChild < heap.Count && heap[i] > heap[rightChild])
                    return false;
            }
            return true;
        }

        static int FindAndRemoveMin(List<int> numbers)
        {
            int minIndex = 0;
            for (int i = 1; i < numbers.Count; i++)
            {
                if (numbers[i] < numbers[minIndex])
                    minIndex = i;
            }
            int minValue = numbers[minIndex];
            numbers.RemoveAt(minIndex);
            return minValue;
        }

        static void Main(string[] args)
        {
            Random random = new Random();

            Console.WriteLine("Unesite donju granicu A: ");
            int a = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Unesite gornju granicu B: ");
            int b = Convert.ToInt32(Console.ReadLine());

            int N = random.Next(1000, 10000001);
            int k = random.Next(10, 101);
           
            List<int> numbers = new List<int>();

            while (numbers.Count < N)
            {
                for (int i = 0; i < k && numbers.Count < N; i++)
                {
                    int newNumber = random.Next(a, b + 1);
                    numbers.Add(newNumber);
                    //Console.WriteLine($"Dodat broj: {newNumber}");
                }

                if (numbers.Count > 1 && numbers.Count!=N)
                {
                    int smallest = FindAndRemoveMin(numbers);
                    //Console.WriteLine($"Izbacen najmanji broj: {smallest}");
                }
            }

            // Ispis generisanog niza
            Console.WriteLine("Generisani niz:");
            numbers.ForEach(x => Console.Write(x + " "));
            Console.WriteLine();

            // Provera da li je niz Min hip
            bool isMinHeap = IsMinHeap(numbers);
            Console.WriteLine($"\nDa li je niz Min hip? {isMinHeap}");
        }

        
    }

}
