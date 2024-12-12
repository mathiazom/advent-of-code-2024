using System.Text.RegularExpressions;

namespace advent_of_code_2024._3._2;

public partial class Program : ISolution
{
    public void Run(string path)
    {
        var text = File.ReadAllText(path);
        var matches = MulRegex().Matches(text);
        var enabled = true;
        Console.WriteLine(
            matches.Select(match =>
            {
                var vals = match.Groups.Values.Select(g => g.Value).ToArray();
                switch (vals[0])
                {
                    case "do()":
                        enabled = true;
                        return 0;
                    case "don't()":
                        enabled = false;
                        return 0;
                }

                return enabled ? int.Parse(vals[1]) * int.Parse(vals[2]) : 0;
            }).Aggregate((acc, curr) => acc + curr)
        );
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)")]
    private static partial Regex MulRegex();
}