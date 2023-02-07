
int batchCount = int.Parse(Console.ReadLine());

for (int i = 0; i < batchCount; i++)
{
    Console.ReadLine();
    string[] strValues = Console.ReadLine().Split();
    int n = int.Parse(strValues[0]);
    int m = int.Parse(strValues[1]);

    int[][] sheet = new int[n][];
    for (int j = 0; j < n; j++)
    {
        sheet[j] = readIntArray(m);
    }
    int clickNum = int.Parse(Console.ReadLine());
    int[] column = readIntArray(clickNum);

    // Prepare rows array
    int[] rowIndex = new int[n];
    for (int k = 0; k < n; k++)
    {
        rowIndex[k] = k;
    }

    // Here Sorting are

    for (int k = 0; k < clickNum; k++)
    {
        Array.Sort(rowIndex, new SheetComparer(sheet, column[k]));
        int[][] sorted = new int[n][];
        for (int l = 0; l < n; l++)
        {
            sorted[l] = sheet[rowIndex[l]];
        }
        sheet = sorted;
    }


    // Print result 
    for (int j = 0; j < n; j++)
    {
        int[] line = new int[m];
        Console.Write(sheet[j][0]);
        for (int k = 1; k < m; k++)
        {
            Console.Write(" {0}", sheet[j][k]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
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

internal class SheetComparer : System.Collections.IComparer
{
    private int[][] Sheet;
    private int ColumnIdx;

    public SheetComparer(int[][] sheet, int v)
    {
        Sheet = sheet;
        ColumnIdx = v - 1;
    }

    public int Compare(object? x, object? y)
    {
        int i = (int)x; int j = (int)y;
        int result = Sheet[i][ColumnIdx].CompareTo(Sheet[j][ColumnIdx]);
        if (result == 0)
            result = i.CompareTo(j);
        return result;
    }
}

