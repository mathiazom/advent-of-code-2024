namespace advent_of_code_2024._21._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var codes = File.ReadAllLines(path);

        var numPathsLookup = ShortestPadPaths([
            [PadState._7, PadState._8, PadState._9],
            [PadState._4, PadState._5, PadState._6],
            [PadState._1, PadState._2, PadState._3],
            [null, PadState._0, PadState.A]
        ]);
        var dirPathsLookup = ShortestPadPaths([
            [null, PadState.Up, PadState.A],
            [PadState.Left, PadState.Down, PadState.Right]
        ]);
        var sum = codes.Sum(c => CodeCost(c, numPathsLookup, dirPathsLookup));

        Console.WriteLine(sum);
        Console.WriteLine(sum == 176452);
    }

    private static int CodeCost(string code,
        Dictionary<PadState, Dictionary<PadState, List<List<PadState>>>> numPathsLookup,
        Dictionary<PadState, Dictionary<PadState, List<List<PadState>>>> dirPathsLookup)
    {
        var stepCount = 0;
        var prev = PadState.A;
        foreach (var num in code.Select(c => c switch
                 {
                     '0' => PadState._0,
                     '1' => PadState._1,
                     '2' => PadState._2,
                     '3' => PadState._3,
                     '4' => PadState._4,
                     '5' => PadState._5,
                     '6' => PadState._6,
                     '7' => PadState._7,
                     '8' => PadState._8,
                     '9' => PadState._9,
                     'A' => PadState.A,
                     _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
                 }))
        {
            stepCount += MinCodeCost(num, prev, numPathsLookup, dirPathsLookup);
            prev = num;
        }

        return stepCount * int.Parse(code[..^1]);
    }

    private static int MinCodeCost(PadState num, PadState npad,
        Dictionary<PadState, Dictionary<PadState, List<List<PadState>>>> numPathsLookup,
        Dictionary<PadState, Dictionary<PadState, List<List<PadState>>>> dirPathsLookup
    )
    {
        var minOptionsNum = int.MaxValue;
        foreach (var pathD1 in numPathsLookup[num][npad])
        {
            var d1 = PadState.A;
            var optionNum = 0;
            foreach (var nextD1 in pathD1)
            {
                var minOptionsD2 = int.MaxValue;
                foreach (var path2D in dirPathsLookup[nextD1][d1])
                {
                    var d2 = PadState.A;
                    var optionD2 = 0;
                    foreach (var next2D in path2D)
                    {
                        optionD2 += dirPathsLookup[next2D][d2][0].Count;
                        if (optionD2 >= minOptionsD2) break;
                        d2 = next2D;
                    }

                    if (optionD2 < minOptionsD2) minOptionsD2 = optionD2;
                }

                optionNum += minOptionsD2;
                if (optionNum >= minOptionsNum) break;
                d1 = nextD1;
            }

            if (optionNum < minOptionsNum) minOptionsNum = optionNum;
        }

        return minOptionsNum;
    }

    private readonly Vec2[] _directions = [new(-1, 0), new(1, 0), new(0, -1), new(0, 1)];

    private Dictionary<PadState, Dictionary<PadState, List<List<PadState>>>> ShortestPadPaths(
        List<List<PadState?>> pad)
    {
        return pad
            .SelectMany(s => s)
            .Where(s => s.HasValue)
            .Select(s => s!.Value)
            .ToDictionary(s => s, s => ShortestPadPathsTo(pad, s));
    }

    private Dictionary<PadState, List<List<PadState>>> ShortestPadPathsTo(List<List<PadState?>> pad, PadState to)
    {
        var map = BuildMap(pad, to, out var startNode);
        Dijkstra(map, startNode);
        return BacktrackMap(map, to);
    }

    private static List<List<Node>> BuildMap(List<List<PadState?>> pad, PadState startState, out Node startNode)
    {
        var map = new List<List<Node>>();
        startNode = null!;
        for (var i = 0; i < pad.Count; i++)
        {
            var row = new List<Node>();
            for (var j = 0; j < pad[i].Count; j++)
            {
                var s = pad[i][j];
                var n = new Node
                {
                    State = s,
                    Pos = new Vec2(j, i)
                };
                row.Add(n);
                if (s != startState) continue;
                startNode = n;
                startNode.Distance = 0;
            }

            map.Add(row);
        }

        return map;
    }

    private void Dijkstra(List<List<Node>> map, Node startNode)
    {
        List<Node> unvisited = [startNode];
        while (unvisited.Count > 0)
        {
            unvisited.Sort((a, b) => a.Distance.CompareTo(b.Distance));
            var node = unvisited[0];
            foreach (var dir in _directions)
            {
                var nx = node.Pos.X + dir.X;
                var ny = node.Pos.Y + dir.Y;
                if (nx < 0 || ny < 0 || nx >= map[0].Count || ny >= map.Count) continue;
                var n = map[ny][nx];
                if (n.State == null) continue;
                var nd = node.Distance + 1;
                if (n.Distance < nd) continue;
                if (!unvisited.Contains(n)) unvisited.Add(n);
                var backtrack = dir.X switch
                {
                    -1 => PadState.Right,
                    1 => PadState.Left,
                    0 => dir.Y switch
                    {
                        -1 => PadState.Down,
                        1 => PadState.Up,
                    }
                };
                if (n.Distance == nd)
                {
                    n.Backtracks.Add(new Backtrack(node, backtrack));
                    continue;
                }

                n.Distance = nd;
                n.Backtracks = [new Backtrack(node, backtrack)];
            }

            unvisited.RemoveAt(0);
        }
    }

    private static Dictionary<PadState, List<List<PadState>>> BacktrackMap(List<List<Node>> map, PadState startState)
    {
        var shortestPaths = new Dictionary<PadState, List<List<PadState>>>();
        foreach (var row in map)
        foreach (var n in row)
        {
            var s = n.State;
            if (!s.HasValue) continue;
            if (s == startState)
            {
                shortestPaths.Add(s.Value, [[PadState.A]]);
                continue;
            }

            var backtracks = new List<List<Backtrack>>();
            backtracks.AddRange(n.Backtracks.Select(b => new List<Backtrack> { b }));
            while (true)
            {
                var finished = true;
                List<List<Backtrack>> extra = [];
                foreach (var b in backtracks)
                {
                    var next = b.Last().Parent.Backtracks;
                    if (next.Count == 0) break;
                    finished = false;
                    if (next.Count > 1)
                    {
                        for (var l = 1; l < next.Count; l++)
                        {
                            extra.Add(b.Append(next[l]).ToList());
                        }
                    }

                    b.Add(next[0]);
                }

                if (finished) break;
                backtracks.AddRange(extra);
            }

            shortestPaths.Add(s.Value,
                backtracks.Select(bs => bs.Select(b => b.BackAction).Append(PadState.A).ToList()).ToList());
        }

        return shortestPaths;
    }

    private class Node
    {
        public PadState? State { get; init; }
        public required Vec2 Pos { get; init; }
        public int Distance { get; set; } = int.MaxValue;
        public List<Backtrack> Backtracks { get; set; } = [];
    }

    private record Backtrack(Node Parent, PadState BackAction);

    private record Vec2(int X, int Y);

    private enum PadState
    {
        A,
        Up,
        Down,
        Left,
        Right,
        _0,
        _1,
        _2,
        _3,
        _4,
        _5,
        _6,
        _7,
        _8,
        _9
    }
}