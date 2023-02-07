int count = int.Parse(Console.ReadLine());
int[] sum = new int[count];

for (int i = 0; i < count; i++)
{
    int buyerCount = int.Parse(Console.ReadLine());
    int[] items = new int[buyerCount];
    int idx = 0;
    foreach (var bougth in Console.ReadLine().Split())
    {
        items[idx] = int.Parse(bougth);
        idx++;
    }

    Array.Sort(items);
    int itemCount = 0; 
    int itemPrice = items[0];
    for (int j = 0; j < buyerCount; j++)
    {
        if (items[j] == itemPrice)
        {
            itemCount++;
        } else {
            sum[i] += itemPrice*(itemCount/3*2 + itemCount%3);
            itemCount = 1;
            itemPrice = items[j];
        }
    }
    sum[i] += itemPrice * (itemCount /3 * 2 + itemCount % 3);
}

for (int i = 0; i < count; i++)
{
    Console.WriteLine(sum[i]);
}

