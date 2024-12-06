using System;
using System.Collections.Generic;

public class BinomialHeap
{
    private class Node
    {
        public int Value;
        public int Degree;
        public Node Parent;
        public Node Sibling;
        public Node Child;

        public Node(int value)
        {
            Value = value;
            Degree = 0;
            Parent = null;
            Sibling = null;
            Child = null;
        }
    }

    private Node _minNode;

    public BinomialHeap()
    {
        _minNode = null;
    }

    public void Insert(int value)
    {
        var newNode = new Node(value);
        var newHeap = new BinomialHeap();
        newHeap._minNode = newNode;
        this.Union(newHeap);
    }

    public void Union(BinomialHeap other)
    {
        if (other._minNode == null)
            return;

        var newRoot = Merge(this._minNode, other._minNode);
        _minNode = newRoot;

        if (_minNode != null)
        {
            Normalize();
        }
    }

    private Node Merge(Node h1, Node h2)
    {
        Node newHead = null, tail = null;

        while (h1 != null || h2 != null)
        {
            Node smallest;
            if (h1 == null)
            {
                smallest = h2;
                h2 = h2.Sibling;
            }
            else if (h2 == null)
            {
                smallest = h1;
                h1 = h1.Sibling;
            }
            else
            {
                if (h1.Degree <= h2.Degree)
                {
                    smallest = h1;
                    h1 = h1.Sibling;
                }
                else
                {
                    smallest = h2;
                    h2 = h2.Sibling;
                }
            }

            if (newHead == null)
            {
                newHead = smallest;
                tail = newHead;
            }
            else
            {
                tail.Sibling = smallest;
                tail = tail.Sibling;
            }
        }

        return newHead;
    }

    private void Normalize()
    {
        if (_minNode == null)
            return;

        Node prev = null;
        Node curr = _minNode;
        Node next = curr.Sibling;

        while (next != null)
        {
            if (curr.Degree != next.Degree || (next.Sibling != null && next.Sibling.Degree == curr.Degree))
            {
                prev = curr;
                curr = next;
            }
            else if (curr.Value <= next.Value)
            {
                curr.Sibling = next.Sibling;
                Link(curr, next);
            }
            else
            {
                if (prev == null)
                    _minNode = next;
                else
                    prev.Sibling = next;
                Link(next, curr);
                curr = next;
            }

            next = curr.Sibling;
        }
    }

    private void Link(Node min, Node other)
    {
        other.Parent = min;
        other.Sibling = min.Child;
        min.Child = other;
        min.Degree++;
    }

    public bool IsMinHeap()
    {
        return IsMinHeap(_minNode);
    }

    private bool IsMinHeap(Node node)
    {
        if (node == null)
            return true;

        if (node.Child != null && node.Value > node.Child.Value)
            return false;

        if (node.Sibling != null && node.Value > node.Sibling.Value)
            return false;

        return IsMinHeap(node.Child) && IsMinHeap(node.Sibling);
    }

    public void DeleteMin()
    {
        if (_minNode == null)
            return;

        Node minPrev = null;
        Node minNode = _minNode;
        Node prev = null;
        Node curr = _minNode;

        while (curr != null)
        {
            if (curr.Value < minNode.Value)
            {
                minNode = curr;
                minPrev = prev;
            }
            prev = curr;
            curr = curr.Sibling;
        }

        if (minPrev == null)
            _minNode = minNode.Sibling;
        else
            minPrev.Sibling = minNode.Sibling;

        Node child = minNode.Child;
        BinomialHeap childHeap = new BinomialHeap();
        while (child != null)
        {
            Node nextChild = child.Sibling;
            child.Sibling = null;
            childHeap.Insert(child.Value);
            child = nextChild;
        }

        this.Union(childHeap);
    }
}

class Program
{
    static Random random = new Random();

    static void Main()
    {
        Console.WriteLine("Unesite donju granicu A: ");
        int a = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Unesite gornju granicu B: ");
        int b = Convert.ToInt32(Console.ReadLine());
        int N = random.Next(1000, 10000001);
        int k = random.Next(10, 101);

        //int a = 3;
        //int b = 15;
        //int N = 15;
        //int k = 5;

        BinomialHeap heap = new BinomialHeap();
        List<int> numbers = new List<int>();

        for (int i = 0; i < N; i++)
        {
            int num = random.Next(a,b+1);
            heap.Insert(num);
            numbers.Add(num);

            if ((i + 1) % k == 0)
            {
                heap.DeleteMin();
                //Console.WriteLine("Obrisan element");
            }
        }

        Console.WriteLine("\nGenerisani niz:");
        numbers.ForEach(x => Console.Write(x + " "));
        Console.WriteLine();

        Console.WriteLine("\nDa li postoji u nizu MinHeap: " + heap.IsMinHeap());
    }
}
