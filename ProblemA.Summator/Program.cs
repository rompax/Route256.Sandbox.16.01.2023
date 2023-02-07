int count = int.Parse(Console.ReadLine());
if (count <= 0) return;
int[] numbers = new int[count];
for (int i = 0; i < count; i++)
{
    foreach (var item in Console.ReadLine().Split())
    {
        numbers[i] += int.Parse(item);
    }
}
for (int i = 0; i < count; i++)
{
    Console.WriteLine(numbers[i]);
}
