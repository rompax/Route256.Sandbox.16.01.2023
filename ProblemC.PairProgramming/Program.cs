int batchCount = int.Parse(Console.ReadLine());
if (batchCount <= 0) return;
for (int i = 0; i < batchCount; i++)
{
    // read input data
    int count = int.Parse(Console.ReadLine());
    int[] skills = new int[count];
    int idx = 0;
    foreach (var item in Console.ReadLine().Split())
    {
        skills[idx] += int.Parse(item); idx++;
    }

    // Prepare data
    Node[] data = new Node[count];
    for (int j = 0; j < count; j++)
    {
        data[j] = new Node()
        {
            InputIndex = j,
            Skill = skills[j],
            Weight = skills[j] * 100 + j
        };
    }
    Array.Sort(data, new SortComparer());
    data[0].NextSorted = data[1];
    data[count - 1].PrevSorted = data[count - 2];
    for (int j = 1; j < count - 1; j++)
    {
        data[j].NextSorted = data[j + 1];
        data[j].PrevSorted = data[j - 1];
    }
    Array.Sort(data, new SetbackComparer());

    // find pairs
    int pairCount = count / 2;
    int[][] pair = new int[pairCount][];
    int k = 0;
    for (int j = 0; j < pairCount; j++)
    {
        pair[j] = new int[2];
        data[k].Processed = true;
        int l = GetPairedIndex(data, k);

        data[l].Processed = true;
        pair[j][0] = data[k].InputIndex + 1;
        pair[j][1] = data[l].InputIndex + 1;

        while (k < count && data[k].Processed) k++;
    }

    for (int j = 0; j < pairCount; j++)
    {
        Console.WriteLine("{0} {1}", pair[j][0], pair[j][1]);
    }
    Console.WriteLine();
}

int FindPrev(Node data)
{
    // find node with skill below current one with max index
    Node prev = GetPrevAnyUnused(data);
    if (prev == null)
        return -1;
    Node prev2 = GetPrevAnyUnused(prev);
    while (prev2 != null && prev2.Skill == prev.Skill)
    {
        prev = prev2;
        prev2 = GetPrevAnyUnused(prev);
    }
    return prev.InputIndex;
}

int FindNext(Node data)
{
    // find node with skill level above current one with min index
    while (data != null && data.Processed)
    {
        data = data.NextSorted;
    }
    if (data == null)
        return -1;
    return data.InputIndex;
}

int GetPairedIndex(Node[] data, int k)
{
    // find nearest nodes by up and below by skill 
    // and choose nearest
    int l;
    int l1 = FindPrev(data[k]);
    int l2 = FindNext(data[k]);
    if (l1 == -1)
        l = l2;
    else if (l2 == -1)
        l = l1;
    else if (data[k].Diff(data[l1]) < data[k].Diff(data[l2]))
        l = l1;
    else if (data[k].Diff(data[l1]) > data[k].Diff(data[l2]))
        l = l2;
    else return Math.Min(l1, l2);
    return l;
}

Node GetPrevAnyUnused(Node data)
{
    // find nearest node with skill below current one
    Node prev = data.PrevSorted;
    while (prev != null && prev.Processed)
    {
        prev = prev.PrevSorted;
    }

    return prev;
}

public class SortComparer : System.Collections.IComparer
{
    public int Compare(Object x, Object y)
    {
        return ((Node)x).Weight.CompareTo(((Node)y).Weight);
    }
}

public class SetbackComparer : System.Collections.IComparer
{
    public int Compare(Object x, Object y)
    {
        return ((Node)x).InputIndex.CompareTo(((Node)y).InputIndex);
    }
}
public class Node
{
    public int Skill;
    public int InputIndex;
    public Node? NextSorted;
    public Node? PrevSorted;
    public bool Processed;
    public int Weight;

    internal int Diff(Node node)
    {
        return Math.Abs(Skill - node.Skill);
    }
}