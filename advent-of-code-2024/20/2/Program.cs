using System.Diagnostics;

namespace advent_of_code_2024._20._2;

public class Program : ISolution
{

    private int _threshold = 100;
    // private int _threshold = 50;
    
    public void Run(string path)
    {
        var count = 0;
        var lines = File.ReadAllLines(path);
        var cheatLookup = new Dictionary<string, int>();

        var baseMap = BuildMap(lines, out var baseUnvisited, out var baseGoalNode);
        Dijkstra(baseMap, baseUnvisited, int.MaxValue, baseGoalNode);
        var baseDistance = baseGoalNode.Distance;
        
        var sw = Stopwatch.StartNew();

        for (var k = 1; k < lines.Length - 1; k++)
        {
            for (var l = 1; l < lines[k].Length - 1; l++)
            {
                if (baseMap[k][l].Type == NodeType.Wall) continue;
                
                Console.WriteLine(l + "," + k + " / " + count + " / " + sw.Elapsed + " / " + cheatLookup.Count);

                var cheat = baseMap[k][l];

                for (int i = -20; i < 21; i++)
                {
                    for (int j = -20+Math.Abs(i); j < 21-Math.Abs(i); j++)
                    {
                        if (i == 0 && j == 0) continue;
                        var cheatEndX = l + j;
                        var cheatEndY = k + i;
                        if (cheatEndY < 0 || cheatEndY >= lines.Length) continue;
                        if (cheatEndX < 0 || cheatEndX >= lines.Length) continue;
                        var baseCheatEnd = baseMap[cheatEndY][cheatEndX];
                        if (baseCheatEnd.Type == NodeType.Wall) continue;
                        
                        var newCheatEnd = cheat.Distance + Math.Abs(i) + Math.Abs(j);
                        if (newCheatEnd >= baseCheatEnd.Distance) continue;
                        
                        var cheatLookupKey = cheatEndX + "," + cheatEndY;
                        if (!cheatLookup.TryGetValue(cheatLookupKey, out var cheatToGoal))
                        {
                            List<List<Node>> submap = [];

                            Node subGoalNode = null!;
                            for (var si = 0; si < lines.Length; si++)
                            {
                                var row = new List<Node>();
                                for (var sj = 0; sj < lines.Length; sj++)
                                {
                                    var n = baseMap[si][sj];
                                    var nn = new Node
                                    {
                                        Type = n.Type,
                                        X = n.X,
                                        Y = n.Y,
                                        Distance = n.Distance,
                                    };
                                    if (nn.X == baseGoalNode.X && nn.Y == baseGoalNode.Y)
                                    {
                                        subGoalNode = nn;
                                    }

                                    row.Add(nn);
                                }

                                submap.Add(row);
                            }
                            var cheatEnd = submap[cheatEndY][cheatEndX];
                            cheatEnd.Distance = newCheatEnd;
                            
                            Dijkstra(submap, [cheatEnd], baseDistance, subGoalNode);
                            cheatToGoal = subGoalNode.Distance - cheatEnd.Distance;
                            cheatLookup.Add(cheatLookupKey, cheatToGoal);
                        }
                        
                        if (baseDistance - (newCheatEnd + cheatToGoal) >= _threshold)
                        {
                            count++;
                        }
                    }
                }
            }
        }

        Console.WriteLine(count);
        Console.WriteLine(count == 987695);
    }

    private readonly Vec2[] _directions = [new(-1, 0), new(1, 0), new(0, -1), new(0, 1)];

    private void Dijkstra(List<List<Node>> map, List<Node> unvisited, int baseDistance, Node goalNode)
    {
        while (unvisited.Count > 0)
        {
            unvisited.Sort((a, b) => a.Distance.CompareTo(b.Distance));
            var node = unvisited[0];
            if (node.Distance != int.MaxValue && node.Distance + Math.Abs(node.X - goalNode.X) + Math.Abs(node.Y - goalNode.Y) > baseDistance) return;
            foreach (var dir in _directions)
            {
                var neighbour = map[node.Y + dir.Y][node.X + dir.X];
                if (neighbour.Type == NodeType.Wall) continue;
                var distance = node.Distance + 1;
                if (neighbour.Distance <= distance) continue;
                neighbour.Distance = distance;
                if (!unvisited.Contains(neighbour)) unvisited.Add(neighbour);
            }

            unvisited.RemoveAt(0);
        }
    }

    private record Vec2(int X, int Y);

    private static List<List<Node>> BuildMap(string[] lines, out List<Node> unvisited, out Node goalNode)
    {
        List<List<Node>> map = [];
        unvisited = [];
        goalNode = null!;

        for (var i = 0; i < lines.Length; i++)
        {
            var row = new List<Node>();
            for (var j = 0; j < lines[i].Length; j++)
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