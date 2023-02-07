int batchCount = int.Parse(Console.ReadLine());

for (int i = 0; i < batchCount; i++)
{
    ReadData()
    .ProcessData()
    .PrintData();
}

Data ReadData()
{
    int dayCount = int.Parse(Console.ReadLine());
    int[] taskNum = readIntArray(dayCount);
    return new Data()
    {
        taskId = taskNum,
        dayCount = dayCount
    };

}

int[] readIntArray(int n)
{
    int[] line = new int[n];
    int index = 0;
    foreach (var item in Console.ReadLine().Split())
    {
        line[index] += int.Parse(item); index++;
    }
    return line;
}

internal class Data
{
    public int dayCount;
    public int[] taskId;
    public bool passed;


    internal Data ProcessData()
    {
        // Build tree and break if same nuber has been processed
        passed = true;
        RedBlackBST<int, int> taskTree = new();
        int curTask = taskId[0];
        taskTree.Insert(curTask, 0);
        for (int i = 1; i < dayCount && passed; i++)
        {
            if (curTask != taskId[i])
            {
                passed = !taskTree.Contains(taskId[i]);
                if (passed)
                {
                    curTask = taskId[i];
                    taskTree.Insert(curTask, 0);
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




