using System.Numerics;

namespace advent_of_code_2024._11._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var line = File.ReadLines(path).First();
        var stones = line.Split(" ").Select(BigInteger.Parse).ToList();
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < stones.Count; j++)
            {
                var stone = stones[j];
                if (stone == 0)
                {
                    stones[j] = 1;
                    continue;
                }

                if (stone.ToString().Length % 2 == 0)
                {
                    var s = stone.ToString();
                    stones[j] = BigInteger.Parse(s[..(s.Length / 2)]);
                    stones.Insert(j+1, BigInteger.Parse(s[(s.Length / 2)..]));
                    j++;
                    continue;
                }
                stones[j] = stone * 2024;
            }
        }
        Console.WriteLine(string.Join(" ", stones));
        Console.WriteLine(stones.Count);
    }
}