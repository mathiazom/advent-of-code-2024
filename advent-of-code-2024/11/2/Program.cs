using System.Numerics;

namespace advent_of_code_2024._11._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var line = File.ReadLines(path).First();
        var stones = line.Split(" ").Select(s => new StoneFamily(BigInteger.Parse(s), new BigInteger(1))).ToList();
        for (var i = 0; i < 75; i++)
        {
            for (var j = 0; j < stones.Count; j++)
            {
                var stone = stones[j];
                if (stone.Value == 0)
                {
                    stones[j].Value = 1;
                    continue;
                }
                var s = stone.Value.ToString();
                if (s.Length % 2 == 0)
                {
                    var left = BigInteger.Parse(s[..(s.Length / 2)]);
                    var right = BigInteger.Parse(s[(s.Length / 2)..]);
                    var leftMerged = false;
                    var rightMerged = false;
                    for (var k = 0; k < j; k++)
                    {
                        if (leftMerged && rightMerged) break;
                        var other = stones[k];
                        if (!leftMerged && other.Value == left)
                        {
                            other.Count += stone.Count;
                            leftMerged = true;
                        }
                        if (!rightMerged && other.Value == right)
                        {
                            other.Count += stone.Count;
                            rightMerged = true;
                        }
                    }

                    if (leftMerged && rightMerged)
                    {
                        stones.RemoveAt(j);
                        j--;
                    }
                    else if (leftMerged)
                    {
                        stones[j].Value = right;
                    }
                    else
                    {
                        stones[j].Value = left;
                        if (!rightMerged)
                        {
                            stones.Insert(j+1, new StoneFamily(right, stone.Count));
                            j++;
                        }
                    }
                    continue;
                }
                stones[j].Value *= 2024;
            }
        }

        var count = stones.Aggregate(new BigInteger(0), (acc, s) => acc + s.Count);
        Console.WriteLine(count);
        Console.WriteLine(count == 221683913164898);
    }

    private class StoneFamily(BigInteger value, BigInteger count)
    {
        public BigInteger Value { get; set; } = value;
        public BigInteger Count { get; set; } = count;
    }
}