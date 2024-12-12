namespace advent_of_code_2024._23._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var connections = File.ReadAllLines(path).Select(l => l.Split("-")).ToList();
        var neighbours = new Dictionary<string, List<string>>();

        foreach (var connection in connections)
        {
            foreach (var c in connection)
            {
                var cn = connection.Where(l => l != c).ToList();
                if (!neighbours.TryGetValue(c, out var n))
                {
                    neighbours.Add(c, cn);
                    continue;
                }

                n.AddRange(cn);
            }
        }

        var maximalCliques = BronKerbosch([], neighbours.Keys.ToList(), [], neighbours);
        var password = string.Join(",", maximalCliques.MaxBy(m => m.Count)!.Order());
        Console.WriteLine(password);
        Console.WriteLine(password == "af,aq,ck,ee,fb,it,kg,of,ol,rt,sc,vk,zh");
    }

    private static List<List<string>> BronKerbosch(List<string> R, List<string> P, List<string> X,
        Dictionary<string, List<string>> N)
    {
        if (P.Count == 0 && X.Count == 0)
            return [R];

        List<List<string>> maxes = [];
        foreach (var v in P.ToList())
        {
            var n = N[v];
            maxes.AddRange(BronKerbosch(R.Append(v).ToList(), P.Where(p => n.Contains(p)).ToList(),
                X.Where(x => n.Contains(x)).ToList(), N));
            P.Remove(v);
            X.Add(v);
        }

        return maxes;
    }
}