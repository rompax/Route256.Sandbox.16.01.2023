int batchCount = int.Parse(Console.ReadLine());


for (int i = 0; i < batchCount; i++)
{
    (new Data())
        .ReadData()
        .ProcessData()
        .PrintData();
}
internal class Data
{
    int lineCount, countInLine, hCount;
    private Hexagon[] hexagons;
    bool passed = true;

    int[] readPair()
    {
        int[] pair = new int[2];
        string[] items = Console.ReadLine().Split(" ");
        pair[0] = int.Parse(items[0]);
        pair[1] = int.Parse(items[1]);
        return pair;
    }
    Queue<int> DefineNeighbours(Hexagon h)
    {
        Queue<int> q = new Queue<int>();
        addIdx(q, h, -1, -1);
        addIdx(q, h, -1, +1);
        addIdx(q, h, 0, -2);
        addIdx(q, h, 0, +2);
        addIdx(q, h, 1, -1);
        addIdx(q, h, 1, +1);
        return q;
    }
    void addIdx(Queue<int> q, Hexagon h, int dRow, int dCol)
    {
        int newRow = h.row + dRow;
        int newCol = h.col + dCol;
        if (newRow >= 0 && newRow < lineCount &&
            newCol >= 0 && newCol < countInLine)
        {
            int idx = (newRow * countInLine + newCol) / 2;
            if (idx >= 0 && idx < hCount) q.Enqueue(idx);
            else throw new Exception("addIdx produces invalid index");
        }
    }

    internal Data ReadData()
    {
        int[] pair0 = readPair();
        lineCount = pair0[0];
        countInLine = pair0[1];
        int idx = 0;
        hCount = (pair0[0] * pair0[1] + pair0[0] % 2) / 2;
        Hexagon[] hexagons = new Hexagon[hCount];
        for (int i = 0; i < pair0[0]; i++)
        {
            string line = Console.ReadLine();
            for (int j = 0; j < pair0[1]; j++)
            {
                if (line[j] == '.') continue;
                hexagons[idx] = new Hexagon(i, j, line[j]) { Index = idx };
                hexagons[idx].Neighbours = DefineNeighbours(hexagons[idx]);
                idx++;

            }
        }
        if (hCount != idx)
            throw new Exception("Hexagons Count calculation error");

        return new Data() { hexagons = hexagons };
    }

    internal Data ProcessData()
    {
        int preprocessed = 0;
        Stack<int> processing = new Stack<int>();
        HashSet<char> usedColors = new HashSet<char>();
        bool[] processed = new bool[hexagons.Length];
        if (hexagons.Length == 0) return this;
        while (preprocessed < hexagons.Length)
        {
            char curColor = hexagons[preprocessed].color;
            if (usedColors.Contains(curColor))
            {
                passed = false;
                break;
            }
            else usedColors.Add(curColor);

            processing.Push(preprocessed);
            while (processing.Count > 0)
            {

                int curIdx = processing.Pop();
                foreach (var item in hexagons[curIdx].Neighbours)
                {
                    if (!processed[item] && hexagons[item].color == curColor)
                        processing.Push(item);
                }
                processed[curIdx] = true;
            }
            while (++preprocessed < hexagons.Length && processed[preprocessed]) ;
        }
        return this;
    }
    internal void PrintData()
    {
        Console.WriteLine(passed ? "YES" : "NO");
    }
}

class Hexagon
{
    public int row;
    public int col;
    public char color;
    public Queue<int> Neighbours;
    public int Index;
    public Hexagon(int row, int col, char color)
    {
        this.row = row;
        this.col = col;
        this.color = color;
    }


    public IEnumerator<int> GetNeighbours()
    {
        return Neighbours.GetEnumerator();
    }
}