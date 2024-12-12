namespace advent_of_code_2024._18._1;

public class Program : ISolution
{
    // private const int Size = 7;
    private const int Size = 71;
    // private const int Falls = 21;
    private const int Falls = 2934;

    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        var falling = lines.Select(l => l.Split(",").Select(int.Parse).ToArray()).ToArray();
        var map = new Node[Size, Size];

        var unvisited = new List<Node>();

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                var n = new Node { NodeType = NodeType.Ok, X = j, Y = i };
                map[i, j] = n;
                unvisited.Add(n);
            }
        }

        for (int i = 0; i < Falls; i++)
        {
            var f = falling[i];
            var n = map[f[1], f[0]];
            n.NodeType = NodeType.Corrupted;
            unvisited.Remove(n);
        }

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Console.Write(map[i, j].NodeType == NodeType.Corrupted ? "#" : ".");
            }

            Console.WriteLine();
        }

        var goalNode = map[Size - 1, Size - 1];

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
        
        Console.WriteLine(goalNode.Distance);
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