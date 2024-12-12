namespace advent_of_code_2024._24._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);

        var lookup = new Dictionary<string, int>();
        var gates = new List<Gate>();

        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if (line.Trim().Length == 0)
            {
                gates = lines[(i + 1)..].Select(g =>
                {
                    var parts = g.Split(" ");
                    return new Gate(parts[0], parts[1], parts[2], parts[4]);
                }).ToList();
                break;
            }

            var parts = line.Split(": ");
            lookup.Add(parts[0], int.Parse(parts[1]));
        }

        var sum = Test(gates, lookup, out var realSum);

        var sumB = sum.ToString("b");
        var realSumB = realSum.ToString("b");
        Console.WriteLine(sumB);
        Console.WriteLine(realSumB);
    }


    private static long Test(List<Gate> gates, Dictionary<string, int> baseLookup, out long realSum)
    {
        var lookup = baseLookup.ToDictionary();
        var _gates = gates.ToList();
        while (true)
        {
            var rest = new List<Gate>();
            var before = _gates.Count;
            foreach (var gate in _gates)
            {
                if (lookup.TryGetValue(gate.Left, out var left) && lookup.TryGetValue(gate.Right, out var right))
                {
                    lookup.Add(gate.Out, gate.Op switch
                    {
                        "AND" => left & right,
                        "OR" => left | right,
                        "XOR" => left ^ right,
                    });
                    continue;
                }

                rest.Add(gate);
            }

            if (rest.Count == before || rest.Count == 0) break;
            _gates = rest;
        }

        realSum = (from l in lookup
            where l.Key.StartsWith('x') || l.Key.StartsWith('y')
            let pos = int.Parse(l.Key[1..3])
            select l.Value * (long)Math.Pow(2, pos)).Sum();

        return (from l in lookup
            where l.Key.StartsWith('z')
            let pos = int.Parse(l.Key[1..3])
            select l.Value * (long)Math.Pow(2, pos)).Sum();
    }

    private record Gate(string Left, string Op, string Right, string Out);
}