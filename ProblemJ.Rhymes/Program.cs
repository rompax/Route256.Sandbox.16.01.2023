(new Data())
        .ReadData()
        .ProcessData()
        .PrintData();

internal class Data
{
    int wordCount;
    string[] words;
    int queryCount;
    string[] queries;
    string[] responses;

    internal void PrintData()
    {
        for (int i = 0; i < responses.Length; i++)
        {
            Console.WriteLine(responses[i]);
        }
    }

    internal Data ProcessData()
    {
        responses = new string[queryCount];
        InitDict(words);
        for (int i = 0; i < queryCount; i++)
        {
            responses[i] = FindWord(queries[i]);
        }
        return this;
    }

    Node root;
    class Node
    {
        internal Dictionary<char, Node> siblings = new Dictionary<char, Node>();
        internal bool completed;
    }
    private void InitDict(string[] words)
    {
        root = new Node();
        foreach (var word in words)
        {
            Node curNode = root;
            for (int i = word.Length - 1; i > -1; i--)
            {
                if (!curNode.siblings.ContainsKey(word[i]))
                    curNode.siblings.Add(word[i], new Node());
                curNode = curNode.siblings[word[i]];
            }
            curNode.completed = true;
        }
    }

    char[] foundChars = new char[4];
    Node[] foundNodes = new Node[4];
    int foundCharsLength = 0;
    private void ClearFoundWord()
    {
        foundCharsLength = 0;
    }

    private void AddCharToFoundWord(char c, Node n)
    {
        int foundCharsCapacity = foundChars.Length;
        if (foundCharsCapacity < foundCharsLength + 1)
        {
            Array.Resize(ref foundChars, foundCharsCapacity * 2);
            Array.Resize(ref foundNodes, foundCharsCapacity * 2);
        }
        foundChars[foundCharsLength] = c;
        foundNodes[foundCharsLength++] = n;
    }

    private string FindWord(string word)
    {
        Node curNode = root;
        ClearFoundWord();

        for (int i = word.Length - 1; i > -1; i--)
        {
            if (curNode.siblings.ContainsKey(word[i]))
            {
                AddCharToFoundWord(word[i], curNode);
                curNode = curNode.siblings[word[i]];
            }
            else
                break;
        }
        if (foundCharsLength == word.Length)
        {
            if (curNode.siblings.Count > 0)
            {
                // move deeper
                char c = curNode.siblings.Keys.First();
                curNode = curNode.siblings[c];
                AddCharToFoundWord(c, curNode);
                FindRestOfWord(curNode);
            }
            else
            {
                do
                {
                    foundCharsLength--;
                    curNode = foundNodes[foundCharsLength];
                } while (curNode != root && curNode.siblings.Count == 1 && !curNode.completed);

                if (!curNode.completed)
                {
                    foreach (var c in curNode.siblings.Keys)
                    {
                        if (c != word[word.Length - 1 - foundCharsLength])
                        {
                            AddCharToFoundWord(c, curNode);
                            curNode = curNode.siblings[c];
                            break;
                        }
                    }
                    FindRestOfWord(curNode);
                }
            }
        }
        else
            FindRestOfWord(curNode);
        Array.Reverse(foundChars, 0, foundCharsLength);
        return (new string(foundChars, 0, foundCharsLength));

        Node FindRestOfWord(Node curNode)
        {
            while (!curNode.completed)
            {
                char c = curNode.siblings.Keys.First();
                curNode = curNode.siblings[c];
                AddCharToFoundWord(c, curNode);
            }

            return curNode;
        }
    }

    internal Data ReadData()
    {
        wordCount = int.Parse(Console.ReadLine());
        words = readArray(wordCount);
        queryCount = int.Parse(Console.ReadLine());
        queries = readArray(queryCount);
        return this;
    }

    string[] readArray(int n)
    {
        string[] items = new string[n];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = Console.ReadLine();
        }
        return items;
    }

}