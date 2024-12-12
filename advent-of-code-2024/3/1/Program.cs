using System.Text.RegularExpressions;

namespace advent_of_code_2024._3._1;

public partial class Program : ISolution
{
    public void Run(string path)
    {
        var text = File.ReadAllText(path);

        var matches = MulRegex().Matches(text);

        Console.WriteLine(
            matches.Select(match =>
            {
                var nums = match.Groups.Values.Select(g => g.Value).ToArray();
                return int.Parse(nums[1]) * int.Parse(nums[2]);
            }).Aggregate((acc, curr) => acc + curr)
        );
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MulRegex();
}