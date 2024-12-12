namespace advent_of_code_2024._16._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        List<Node> goalNodes = [];
        var map = new List<List<List<Node>>>();
        var unvisited = new List<Node>();
        for (var i = 0; i < lines.Length; i++)
        {
            var row = new List<List<Node>>();
            for (var j = 0; j < lines[i].Length; j++)
            {
                var c = lines[i][j];
                var node = new Node
                {
                    Type = c switch
                    {
                        '.' or 'S' => NodeType.Empty,
                        '#' => NodeType.Wall,
                        'E' => NodeType.Goal,
                        _ => throw new Exception("Invalid tile")
                    },
                    Distance = c == 'S' ? 0 : int.MaxValue,
                    X = j,
                    Y = i
                };

                List<Node> nodeDirections =
                [
                    new()
                    {
                        Type = node.Type,
                        Distance = node.Distance,
                        X = node.X,
                        Y = node.Y,
                        Direction = new Vec2(-1, 0)
                    },
                    new()
                    {
                        Type = node.Type,
                        Distance = node.Distance,
                        X = node.X,
                        Y = node.Y,
                        Direction = new Vec2(1, 0)
                    },
                    new()
                    {
                        Type = node.Type,
                        Distance = node.Distance,
                        X = node.X,
                        Y = node.Y,
                        Direction = new Vec2(0, -1)
                    },
                    new()
                    {
                        Type = node.Type,
                        Distance = node.Distance,
                        X = node.X,
                        Y = node.Y,
                        Direction = new Vec2(0, 1)
                    }
                ];
                
                if (node.Type != NodeType.Wall)
                {
                    if (node.Type == NodeType.Goal)
                    {
                        goalNodes.AddRange(nodeDirections);
                    }
                }

                row.Add(nodeDirections);
                if (c == 'S')
                {
                    unvisited.Add(nodeDirections[1]);
                }
            }
            map.Add(row);
        }

        if (goalNodes.Count == 0)
        {
            Console.WriteLine("No goal");
            return;
        }

        while (unvisited.Count > 0)
        {
            unvisited.Sort((a, b) => a.Distance.CompareTo(b.Distance));
            var node = unvisited[0];
            if (node.Type == NodeType.Goal || node.Distance == int.MaxValue) break;
            foreach (var dir in (Vec2[]) [new Vec2(-1, 0), new Vec2(1, 0), new Vec2(0, -1), new Vec2(0, 1)])
            {
                var nodeDirection = node.Direction;
                if (nodeDirection.X == -dir.X && nodeDirection.Y == -dir.Y) continue; // no point going back
                var nX = node.X + dir.X;
                var nY = node.Y + dir.Y;
                var nC = lines[nY][nX];
                if (nC == '#') continue;
                var neighbour = map[nY][nX].FirstOrDefault(n => n.Direction == dir);
                if (neighbour == null) continue;
                if (neighbour.Type == NodeType.Wall) continue; // can't go through walls
                var sameDirection = nodeDirection.X == dir.X && nodeDirection.Y == dir.Y;
                var distance = node.Distance + (sameDirection ? 1 : 1001);
                if (neighbour.Distance <= distance) continue;
                neighbour.Distance = distance;
                neighbour.Parent = node;
                if (!unvisited.Contains(neighbour))
                {
                    unvisited.Add(neighbour);
                }
            }
            unvisited.Remove(node);
        }

        Console.WriteLine(goalNodes.Min(g => g.Distance));
        Console.WriteLine(goalNodes.Min(g => g.Distance) == 102504);
    }

    public enum NodeType
    {
        Wall,
        Empty,
        Goal
    }

    public class Node
    {
        public NodeType Type { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
        public Node? Parent { get; set; }
        public int Distance { get; set; }
        public Vec2 Direction { get; set; }
    }

    public record Vec2(int X, int Y);

    public record Visit(Node Node, List<Vec2> Directions);
}