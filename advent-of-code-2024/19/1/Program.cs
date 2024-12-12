namespace advent_of_code_2024._19._1;

public class Program: ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        var patterns = lines[0].Split(", ");
        var designs = lines[2..];
        
        var lookup = new Dictionary<string, bool>();
        var count = designs.Count(design => IsPossible(design, patterns, lookup));

        Console.WriteLine(count);
        Console.WriteLine(count == 267);
    }

    private static bool IsPossible(string design, string[] patterns, Dictionary<string, bool> lookup)
    {
        var p = false;
        foreach (var pattern in patterns)
        {
            if (lookup.TryGetValue(design, out var l))
            {
                p = p || l;
                continue;
            }
            if (pattern == design)
                return true;

            if (!design.StartsWith(pattern)) continue;
            var dp = IsPossible(design[pattern.Length..], patterns, lookup);
            p = p || dp;
        }
        lookup[design] = p;

        return p;
    }
}