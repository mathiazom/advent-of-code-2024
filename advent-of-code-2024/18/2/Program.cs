namespace advent_of_code_2024._18._2;

public class Program : ISolution
{
    // private const int Size = 7;
    private const int Size = 71;

    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        var falling = lines.Select(l => l.Split(",").Select(int.Parse).ToArray()).ToArray();
        
        var map = new Node[Size, Size];

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                var n = new Node { NodeType = NodeType.Ok, X = j, Y = i };
                map[i, j] = n;
            }
        }

        var goalNode = map[Size - 1, Size - 1];

        var falls = falling.Length / 2;

        var maxOk = 0;
        
        while (true)
        {
            
            Console.WriteLine(falls);
            var unvisited = new List<Node>();
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var x = map[i, j];
                    x.NodeType = NodeType.Ok;
                    x.Distance = int.MaxValue;
                    x.Parent = null;
                    if (x.NodeType == NodeType.Ok)
                    {
                        unvisited.Add(x);
                    }
                }
            }
            
            foreach (var f in falling[..falls])
            {
                var n = map[f[1], f[0]];
                n.NodeType = NodeType.Corrupted;
            }
            
            map[0, 0].Distance = 0;
            
            while (true)
            {
                if (unvisited.Count == 0)
                {
                    break;
                }

                unvisited.Sort((a, b) => a.Distance.CompareTo(b.Distance));
                var node = unvisited[0];
                if (node.Distance == int.MaxValue) break;
                foreach (var dir in new[] { new Vec2(-1, 0), new Vec2(1, 0), new Vec2(0, 1), new Vec2(0, -1) })
                {
                    if (node.X + dir.X >= Size || node.X + dir.X < 0 || node.Y + dir.Y >= Size ||
                        node.Y + dir.Y < 0) continue;
                    var neighbour = map[node.Y + dir.Y, node.X + dir.X];
                    if (neighbour.NodeType == NodeType.Corrupted) continue;
                    var distance = node.Distance + 1;
                    if (neighbour.Distance > distance)
                    {
                        neighbour.Distance = distance;
                        neighbour.Parent = node;
                    }
                }
                unvisited.RemoveAt(0);
            }

            if (goalNode.Distance == int.MaxValue)
            {
                if (falls == maxOk + 1)
                {
                    var c = string.Join(",", falling[falls - 1]);
                    Console.WriteLine("STOP");
                    Console.WriteLine(c);
                    Console.WriteLine(c == "6,36");
                    break;
                }
                falls = (maxOk + falls) / 2;
                continue;
            }

            maxOk = falls;
            falls = (falls + falling.Length) / 2;
        }
    }

    public class Node
    {
        public NodeType NodeType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Distance { get; set; } = int.MaxValue;
        public Node? Parent { get; set; }
    }

    public record Vec2(int X, int Y);


    public enum NodeType
    {
        Corrupted,
        Ok
    }
}