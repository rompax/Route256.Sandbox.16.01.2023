int batchCount = int.Parse(Console.ReadLine());

for (int i = 0; i < batchCount; i++)
{
    ReadData()
    .ProcessData()
    .PrintData();
}

Data ReadData()
{
    int count = int.Parse(Console.ReadLine());
    TimeInterval[] intervals = readIntervalArray(count);
    return new Data()
    {
        Intervals = intervals,
        Count = count,
        passed= IsValid(intervals)
    };
}

bool IsValid(TimeInterval[] intervals)
{
    bool isValid = true;
    for (int i = 0; i < intervals.Length && isValid; i++)
        isValid = intervals[i].IsValid;
    return isValid;
}

TimeInterval[] readIntervalArray(int n)
{
    TimeInterval[] lines = new TimeInterval[n];
    for (int i = 0; i < n; i++)
    {
        string[] items = Console.ReadLine().Split("-");

        lines[i] = new TimeInterval();
        try
        {
            lines[i].Start = readTime(items[0]);
            lines[i].End = readTime(items[1]);
        }
        catch (Exception)
        {
            lines[i].IsValid = false;
        }
        if (lines[i].Start > lines[i].End) lines[i].IsValid = false;
    }
    return lines;
}

TimeSpan readTime(string v)
{
    return Convert.ToDateTime(v).TimeOfDay;
}

internal class Data
{
    public int Count;
    public TimeInterval[] Intervals;
    public bool passed = true;


    internal Data ProcessData()
    {
        // Build tree and break if intersecion is found
        if (!passed) return this;
        RedBlackBST<TimeInterval, int> tree = new();
        TimeInterval curTask = Intervals[0];
        tree.Insert(curTask, 0);
        for (int i = 1; i < Count && passed; i++)
        {
            if (curTask != Intervals[i])
            {
                try
                {
                    curTask = Intervals[i];
                    tree.Insert(curTask, 0);
                }
                catch (Exception)
                {
                    passed = false;
                }
            }
        }
        return this;
    }

    internal void PrintData()
    {
        Console.WriteLine(passed ? "YES" : "NO");
    }
}

class TimeInterval : IComparable
{
    public TimeSpan Start, End;
    public bool IsValid = true;

    public int CompareTo(object? obj)
    {
        if (obj == null) throw new ArgumentNullException();
        TimeInterval toCheck = (TimeInterval)obj;
        if (toCheck == null) throw new ArgumentException();
        if (toCheck.Start > End) return 1;
        if (toCheck.End < Start) return -1;
        //if (toCheck.Start == Start &&
        //    toCheck.End == End) return 0;
        throw new Exception();
    }
}

public class RedBlackBST<Key, Value> : Object where Key : IComparable
    // LLRB tree by ROBERT SEDGEWICK 
{
    private static readonly bool RED = true;
    private static readonly bool BLACK = false;
    private Node? root;

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

    private Node put(Node? h, Key key, Value val)
    {
        if (h == null) return new Node(key, val, RED);
        int cmp = key.CompareTo(h.key);
        if (cmp < 0) h.left = put(h.left, key, val);
        else if (cmp > 0) h.right = put(h.right, key, val);
        else if (cmp == 0) h.val = val;
        if (isRed(h.right) && !isRed(h.left)) h = rotateLeft(h);
        if (isRed(h.left) && isRed(h.left.left)) h = rotateRight(h);
        if (isRed(h.left) && isRed(h.right)) flipColors(h);

        return h;
    }

    public void Insert(Key key, Value val)
    {
        root = put(root, key, val);
    }

    public bool Contains(Key key)
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
    private class Node
    {
        internal Key key;
        internal Value val;
        internal Node? left, right;
        internal bool color; // color of parent link
        internal Node(Key key, Value val, bool color)
        {
            this.key = key;
            this.val = val;
            this.color = color;
        }
    }
}