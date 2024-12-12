namespace advent_of_code_2024._16._2;

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
                var neighbour = map[nY][nX].FirstOrDefault(n => n.Direction.X == dir.X && n.Direction.Y == dir.Y);
                if (neighbour == null) continue;
                if (neighbour.Type == NodeType.Wall) continue; // can't go through walls
                var sameDirection = nodeDirection.X == dir.X && nodeDirection.Y == dir.Y;
                var distance = node.Distance + (sameDirection ? 1 : 1001);
                if (neighbour.Distance < distance) continue;
                if (neighbour.Distance == distance)
                {
                    Console.WriteLine("N1 :" + neighbour.Distance + ": " + string.Join(",",neighbour.Parents.Select(p => p.X + "," + p.Y)));
                    neighbour.Parents.Add(node);
                    Console.WriteLine("N2 :" + neighbour.Distance + ": " + string.Join(",",neighbour.Parents.Select(p => p.X + "," + p.Y)));
                }
                else
                {
                    neighbour.Parents = [node];
                    neighbour.Distance = distance;
                }
                if (!unvisited.Contains(neighbour))
                {
                    unvisited.Add(neighbour);
                }
                if (neighbour.Type == NodeType.Goal)
                {
                    Console.WriteLine("GOAL! " + neighbour.Parents.Count);
                }
            }
            unvisited.Remove(node);
        }

        var back = goalNodes;
        Console.WriteLine(string.Join(',', back.Select(n => n.X + "," + n.Y)));
        while (true)
        {
            var before = back.Count;
            List<Node> extra = [];
            foreach (var b in back)
            {
                extra.AddRange(b.Parents);
            }
            back.AddRange(extra);
            back = back.DistinctBy(n => n.X + "," + n.Y + "," + n.Direction).ToList();
            if (before == back.Count) break;
            Console.WriteLine(back.Count);
        }
        back = back.DistinctBy(n => n.X + "," + n.Y).ToList();

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                if (back.Any(n => n.X == j && n.Y == i))
                {
                    Console.Write("O");
                }
                else
                {
                    Console.Write(lines[i][j]);
                }
            }
            Console.WriteLine();
        }

        back.Sort((a, b) =>
        {
            var y = a.Y - b.Y;
            if (y != 0) return y;
            return a.X - b.X;
        });
        

        Console.WriteLine(goalNodes.Min(g => g.Distance));
        Console.WriteLine(goalNodes.Min(g => g.Distance) == 102504);
        Console.WriteLine(back.Count);
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
        public List<Node> Parents { get; set; } = [];
        public int Distance { get; set; }
        public Vec2 Direction { get; set; }
    }

    public record Vec2(int X, int Y);

    public record Visit(Node Node, List<Vec2> Directions);
}