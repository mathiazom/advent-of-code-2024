using System.Numerics;

namespace advent_of_code_2024._19._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        var patterns = lines[0].Split(", ");
        var designs = lines[2..];

        var lookup = new Dictionary<string, BigInteger>();
        var count = designs.Aggregate<string, BigInteger>(0, (current, design) => current + CountSolutions(design, patterns, lookup));

        Console.WriteLine(count);
        Console.WriteLine(count == 796449099271652);
    }

    private static BigInteger CountSolutions(string design, string[] patterns, Dictionary<string, BigInteger> lookup)
    {
        BigInteger c = 0;

        foreach (var pattern in patterns)
        {
            if (lookup.TryGetValue(design + ";" + pattern, out var s))
            {
                c += s;
                continue;
            }

            if (pattern == design)
            {
                c++;
                continue;
            }

            if (!design.StartsWith(pattern)) continue;
            var cs = CountSolutions(design[pattern.Length..], patterns, lookup);
            lookup[design + ";" + pattern] = cs;
            c += cs;
        }

        return c;
    }
}