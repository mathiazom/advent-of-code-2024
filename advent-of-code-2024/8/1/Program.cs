namespace advent_of_code_2024._8._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        
        var ants = new Dictionary<char, List<Pos>>();
        
        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines[0].Length; j++)
            {
                var c = lines[i][j];
                if (c == '.') continue;
                var pos = new Pos(j, i);
                if (ants.TryGetValue(c, out var list))
                {
                    list.Add(pos);
                    continue;
                }
        
                ants.Add(c, [pos]);
            }
        }
        
        var ans = new List<Pos>();
        
        foreach (var fq in ants)
        {
            var fants = fq.Value;
            for (var i = 0; i < fants.Count; i++)
            {
                var a = fants[i];
                for (int j = i + 1; j < fants.Count; j++)
                {
                    var b = fants[j];
                    var dx = a.X - b.X;
                    var dy = a.Y - b.Y;
                    var an1 = new Pos(a.X + dx, a.Y + dy);
                    var an2 = new Pos(b.X - dx, b.Y - dy);
                    Console.WriteLine($"[{a}, {b}] -> [{dx},{dy}] -> [{an1}, {an2}]");
                    if (an1.X >= 0 && an1.X < lines.Length && an1.Y >= 0 && an1.Y < lines.Length && !ans.Contains(an1)) ans.Add(an1);
                    if (an2.X >= 0 && an2.X < lines.Length && an2.Y >= 0 && an2.Y < lines.Length && !ans.Contains(an2)) ans.Add(an2);
                }
            }
        }
        
        Console.WriteLine(string.Join("\n", ants.Select(p => p.Key + ": " + string.Join(",", p.Value))));
        Console.WriteLine(string.Join("\n", ans));
        Console.WriteLine(ans.Count);
    }
}

internal record Pos(int X, int Y)
{
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}