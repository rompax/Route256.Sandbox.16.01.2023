ReadData()
.ProcessData()
.PrintData();

Data ReadData()
{
    int[] pair0 = readPair();
    Person[] persons = new Person[pair0[0]];
    for (int i = 0; i < pair0[0]; i++)
    {
        persons[i] = new Person() { Id = i + 1 };
    }
    for (int i = 0; i < pair0[1]; i++)
    {
        int[] pair = readPair();
        pair[0]--; pair[1]--;
        persons[pair[0]].AddFriend(persons[pair[1]]);
        persons[pair[1]].AddFriend(persons[pair[0]]);
    }

    return new Data(persons);
}

int[] readPair()
{
    int[] pair = new int[2];
    string[] items = Console.ReadLine().Split(" ");
    pair[0] = int.Parse(items[0]);
    pair[1] = int.Parse(items[1]);
    return pair;
}

internal class Data
{
    public int Count;
    public Person[] Persons;
    public Data(Person[] p)
    {
        Persons = p;
        Count = p.Length;
    }
    internal Data ProcessData()
    {
        foreach (var p in Persons)
        {
            p.FindSuggested();
        }
        return this;
    }

    internal void PrintData()
    {
        for (int i = 0; i < Persons.Length; i++)
        {
            Persons[i].PrintData();
        }
    }
}

public class Person : IComparable
{
    public int Id;
    public int FriendsCount = 0;
    public Person[] Friends = new Person[5];
    public int SuggestedCount = 0;
    public int[] Suggested;

    public int CompareTo(object? obj)
    {
        Person? p = obj as Person;
        if (p == null)
            return -1;
        return Id.CompareTo(p.Id);
    }

    internal void AddFriend(Person p)
    {
        Friends[FriendsCount] = p;
        FriendsCount++;
    }

    internal void FindSuggested()
    {
        RedBlackBST firstCircle = new RedBlackBST();
        for (int i = 0; i < FriendsCount; i++)
            firstCircle.Insert(Friends[i]);
        RedBlackBST secondCircle = new RedBlackBST();
        for (int i = 0; i < FriendsCount; i++)
        {
            Person p = Friends[i];
            for (int j = 0; j < p.FriendsCount; j++)
            {
                if (!firstCircle.Contains(p.Friends[j]) && Id != p.Friends[j].Id)
                    secondCircle.Insert(p.Friends[j]);
            }
        }
        if (secondCircle.Count == 0)
            return;
        int max = 0;
        Stack<RedBlackBST.Node> suggestedStack = new Stack<RedBlackBST.Node>();
        foreach (RedBlackBST.Node item in secondCircle)
        {
            if (item.val > max)
            {
                suggestedStack.Clear();
                max = item.val;
            }
            if (item.val == max)
                suggestedStack.Push(item);
        }
        SuggestedCount = suggestedStack.Count;
        Suggested = new int[SuggestedCount];
        for (int i = 0; i < SuggestedCount; i++)
        {
            Suggested[i] = suggestedStack.Pop().key.Id;
        }
        Array.Sort(Suggested);
    }

    internal void PrintData()
    {
        if (SuggestedCount == 0)
            Console.WriteLine(0);
        else
        {
            Array.Sort(Suggested);
            Console.Write(Suggested[0]);
            for (int i = 1; i < SuggestedCount; i++)
                Console.Write(" {0}", Suggested[i]);
            Console.WriteLine();
        }
    }
}

public class RedBlackBST : IEnumerable<RedBlackBST.Node>
// based on LLRB BSTree by ROBERT SEDGEWICK 
{
    private static readonly bool RED = true;
    private static readonly bool BLACK = false;
    private Node? root;
    public int Count = 0;

    private bool isRed(Node? x)
    {
        if (x == null) return false;
        return x.color == RED;
    }

    private Node rotateLeft(Node h)
    {
        Node? x = h.right;
        h.right = x.left;
        x.left = h;
        x.color = h.color;
        h.color = RED;
        return x;
    }

    private Node rotateRight(Node h)
    {
        Node? x = h.left;
        h.left = x.right;
        x.right = h;
        x.color = h.color;
        h.color = RED;
        return x;
    }

    private void flipColors(Node h)
    {
        h.color = RED;
        h.left.color = BLACK;
        h.right.color = BLACK;
    }

    private Node add(Node? h, Person key, int val)
    {
        if (h == null)
        {
            Count++;
            return new Node(key, val, RED);
        }
        int cmp = key.CompareTo(h.key);
        if (cmp < 0) h.left = add(h.left, key, val);
        else if (cmp > 0) h.right = add(h.right, key, val);
        else if (cmp == 0) h.val += val;
        if (isRed(h.right) && !isRed(h.left)) h = rotateLeft(h);
        if (isRed(h.left) && isRed(h.left.left)) h = rotateRight(h);
        if (isRed(h.left) && isRed(h.right)) flipColors(h);

        return h;
    }

    public void Insert(Person key)
    {
        root = add(root, key, 1);
    }

    public bool Contains(Person key)
    {
        Node? x = root;
        while (x != null)
        {
            int cmp = key.CompareTo(x.key);
            if (cmp < 0) x = x.left;
            else if (cmp > 0) x = x.right;
            else if (cmp == 0) return true;
        }
        return false;
    }
    public IEnumerator<Node> GetEnumerator()
    {
        return new TreeEnumerator(root);
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class TreeEnumerator : IEnumerator<Node>
    {
        Stack<Node> stack = new Stack<Node>();
        Node root;

        public TreeEnumerator(Node root)
        {
            this.root = root;
            stack.Push(root);
        }
        public Node Current
        {
            get
            {
                return current;
            }
        }

        public Node? current = null;

        object System.Collections.IEnumerator.Current
        {
            get
            {
                return current;
            }
        }

        public void Dispose()
        {
            stack.Clear();
        }

        public bool MoveNext()
        {
            if (stack.Count == 0)
            {
                current = null;
                return false;
            }
            current = stack.Pop();
            if (current.left != null) stack.Push(current.left);
            if (current.right != null) stack.Push(current.right);
            return true;
        }

        public void Reset()
        {
            stack.Clear();
            stack.Push(root);
        }
    }


    public class Node
    {
        internal Person key;
        internal int val;
        internal Node? left, right;
        internal bool color; // color of parent link
        internal Node(Person key, int val, bool color)
        {
            this.key = key;
            this.val = val;
            this.color = color;
        }
    }
}

