namespace advent_of_code_2024._24._1;

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

        while (true)
        {
            var rest = new List<Gate>();
            foreach (var gate in gates)
            {
                if (!lookup.TryGetValue(gate.Left, out var left) || !lookup.TryGetValue(gate.Right, out var right))
                {
                    rest.Add(gate);
                    continue;
                }

                lookup.Add(gate.Out, gate.Op switch
                {
                    "AND" => left & right,
                    "OR" => left | right,
                    "XOR" => left ^ right,
                });
            }

            if (rest.Count == 0) break;
            gates = rest;
        }

        var output = (from l in lookup
            where l.Key.StartsWith('z')
            let pos = int.Parse(l.Key[1..3])
            select l.Value * (long)Math.Pow(2, pos)).Sum();


        Console.WriteLine(output);
        Console.WriteLine(output == 56939028423824);
    }

    private record Gate(string Left, string Op, string Right, string Out);
}