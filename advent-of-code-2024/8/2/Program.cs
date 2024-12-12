using advent_of_code_2024;

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
                    for (var k = 0; k < lines.Length; k++)
                    {
                        var an = new Pos(a.X + k * dx, a.Y + k * dy);
                        if (an.X < 0 || an.X >= lines.Length || an.Y < 0 || an.Y >= lines.Length)
                        {
                            break;
                        }
        
                        if (!ans.Contains(an)) ans.Add(an);
                    }
                    for (var k = 0; k < lines.Length; k++)
                    {
                        var an = new Pos(b.X - k * dx, b.Y - k * dy);
                        if (an.X < 0 || an.X >= lines.Length || an.Y < 0 || an.Y >= lines.Length)
                        {
                            break;
                        }
        
                        if (!ans.Contains(an)) ans.Add(an);
                    }
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