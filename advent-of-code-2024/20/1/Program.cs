namespace advent_of_code_2024._20._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var count = 0;
        var lines = File.ReadAllLines(path);

        var baseMap = BuildMap(lines, out var baseUnvisited, out var baseGoalNode);
        Dijkstra(baseMap, baseUnvisited, int.MaxValue, baseGoalNode);
        var baseDistance = baseGoalNode.Distance;

        for (int k = 1; k < lines.Length - 1; k++)
        {
            for (int l = 1; l < lines[k].Length - 1; l++)
            {
                if (baseMap[k][l].Type != NodeType.Wall)
                {
                    continue;
                }

                List<List<Node>> map = new List<List<Node>>();

                Node goalNode = null!;
                for (int i = 0; i < baseMap.Count; i++)
                {
                    var row = new List<Node>();
                    for (int j = 0; j < baseMap[i].Count; j++)
                    {
                        var n = baseMap[i][j];
                        var nn = new Node
                        {
                            Type = n.Type,
                            X = n.X,
                            Y = n.Y,
                            Distance = n.Distance,
                        };
                        if (nn.X == baseGoalNode.X && nn.Y == baseGoalNode.Y)
                        {
                            goalNode = nn;
                        }

                        row.Add(nn);
                    }

                    map.Add(row);
                }

                var cheat = map[k][l];
                cheat.Type = NodeType.Track;

                var cheatDistance = cheat.Distance;
                foreach (var dir in new Vec2[] { new(-1, 0), new(1, 0), new(0, -1), new(0, 1) })
                {
                    var d = map[k + dir.Y][l + dir.X];
                    if (d.Type == NodeType.Wall) continue;
                    if (d.Distance + 1 < cheatDistance)
                    {
                        cheatDistance = d.Distance + 1;
                    }
                }

                cheat.Distance = cheatDistance;

                List<Node> unvisited = [cheat];

                var check = Dijkstra(map, unvisited, baseDistance, goalNode);

                if (check && baseDistance - goalNode.Distance >= 100)
                {
                    count++;
                }
            }
        }

        Console.WriteLine(count);
        Console.WriteLine(count == 1332);
    }

    private bool Dijkstra(List<List<Node>> map, List<Node> unvisited, int baseDistance, Node goalNode)
    {
        while (unvisited.Count > 0)
        {
            unvisited.Sort((a, b) => a.Distance.CompareTo(b.Distance));
            var node = unvisited[0];
            foreach (var dir in new Vec2[] { new(-1, 0), new(1, 0), new(0, -1), new(0, 1) })
            {
                var neighbour = map[node.Y + dir.Y][node.X + dir.X];
                if (neighbour.Type == NodeType.Wall) continue;
                var distance = node.Distance + 1;
                if (neighbour.Distance > distance)
                {
                    neighbour.Distance = distance;
                    if (!unvisited.Contains(neighbour))
                    {
                        unvisited.Add(neighbour);
                    }
                }
            }

            unvisited.RemoveAt(0);
        }

        return true;
    }

    private record Vec2(int X, int Y);

    private List<List<Node>> BuildMap(string[] lines, out List<Node> unvisited, out Node goalNode)
    {
        List<List<Node>> map = [];

        unvisited = [];

        goalNode = null!;

        for (int i = 0; i < lines.Length; i++)
        {
            var row = new List<Node>();
            for (int j = 0; j < lines[i].Length; j++)
            {
                var type = lines[i][j] switch
                {
                    '.' => NodeType.Track,
                    '#' => NodeType.Wall,
                    'S' => NodeType.Start,
                    'E' => NodeType.Goal,
                    _ => throw new ArgumentOutOfRangeException()
                };
                var node = new Node
                {
                    X = j,
                    Y = i,
                    Type = type,
                    Distance = type == NodeType.Start ? 0 : int.MaxValue
                };
                row.Add(node);
                if (type != NodeType.Wall)
                {
                    unvisited.Add(node);
                }

                if (type == NodeType.Goal)
                {
                    goalNode = node;
                }
            }

            map.Add(row);
        }

        if (goalNode == null)
        {
            throw new Exception("No goal");
        }

        return map;
    }

    private class Node
    {
        public NodeType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Distance { get; set; }
    }

    private enum NodeType
    {
        Start,
        Track,
        Wall,
        Goal,
    }
}