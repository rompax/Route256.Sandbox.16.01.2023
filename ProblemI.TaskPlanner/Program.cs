
(new Data())
        .ReadData()
        .ProcessData()
        .PrintData();

internal class Data
{
    int ProcCount, TaskCount;
    ulong consumation = 0;
    ProcTask[] tasks;
    int[] power;

    internal void PrintData()
    {
        Console.WriteLine(consumation);
    }

    internal Data ProcessData()
    {
        consumation = 0;
        ProcTask[] stack = new ProcTask[TaskCount * 2];

        for (int i = 0; i < TaskCount; i++)
        {
            EndEvent end = new EndEvent(tasks[i]);
            tasks[i].SetEnd(end);
            stack[i * 2] = end;
            stack[i * 2 + 1] = tasks[i];
        }

        Array.Sort(stack, (x, y) =>
        {
            int result = x.moment.CompareTo(y.moment);
            if (result == 0)
            {
                int valx = x.moment; int valy = y.moment;
                if (x is EndEvent) valx--;
                if (y is EndEvent) valy--;
                result = valx.CompareTo(valy);
            }
            return result;
        });

        InitProcs();
        for (int i = 0; i < TaskCount * 2; i++)
        {
            if (stack[i] is EndEvent)
            {
                EndEvent e = (EndEvent)stack[i];
                if (!e.dropped)
                {
                    consumation += Convert.ToUInt64(e.duration) * Convert.ToUInt64(power[e.processor]);
                    LeaveProc(e.processor);
                }
            }
            else
            {
                int proc = TakeProc();
                if (proc < 0) stack[i].DropTask();
                else
                {
                    stack[i].TakeProc(proc);
                }
            }
        }
        return this;
    }

    PriorityQueue<int, int> PriorityQueue;

    private void InitProcs()
    {
        PriorityQueue = new PriorityQueue<int, int>(ProcCount);
        for (int i = 0; i < ProcCount; i++)
            PriorityQueue.Enqueue(i, power[i]);
    }


    private int TakeProc()
    {
        if (PriorityQueue.Count == 0) return -1;
        return PriorityQueue.Dequeue();
    }

    private void LeaveProc(int processor)
    {
        PriorityQueue.Enqueue(processor, power[processor]);
    }

    internal Data ReadData()
    {
        int[] settings = readArray(2);
        ProcCount = settings[0];
        TaskCount = settings[1];
        power = readArray(ProcCount);
        tasks = new ProcTask[TaskCount];
        for (int i = 0; i < tasks.Length; i++)
        {
            int[] taskSettings = readArray(2);
            tasks[i] = new ProcTask() { moment = taskSettings[0], duration = taskSettings[1] };
        }
        return this;
    }
    int[] readArray(int n)
    {
        int[] pair = new int[n];
        string[] items = Console.ReadLine().Split(" ");
        for (int i = 0; i < items.Length; i++)
        {
            pair[i] = int.Parse(items[i]);
        }
        return pair;
    }

    public class Event
    {
        public int moment;
    }
    public class ProcTask : Event
    {
        internal int duration;
        EndEvent End;
        internal void DropTask()
        {
            if (End != null)
                End.dropped = true;
        }
        internal void TakeProc(int p)
        {
            if (End != null)
                End.processor = p;
        }
        internal void SetEnd(EndEvent end)
        {
            End = end;
        }
    }
    public class EndEvent : ProcTask
    {
        internal bool dropped;
        internal int processor;

        public EndEvent(ProcTask task)
        {
            dropped = false;
            processor = 0;
            duration = task.duration;
            moment = task.moment + duration;
        }
    }
}
