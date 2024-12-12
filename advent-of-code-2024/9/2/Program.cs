using System.Numerics;

namespace advent_of_code_2024._9._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var line = File.ReadLines(path).First();
        var blocks = new List<List<string>>();
        for (var i = 0; i < line.Length; i++)
        {
            var c = int.Parse(line[i].ToString());
            if (c == 0) continue;
            blocks.Add(Enumerable.Repeat(i % 2 == 0 ? (i / 2).ToString() : ".", c).ToList());
        }

        for (var i = blocks.Count - 1; i >= 0; i--)
        {
            var block = blocks[i].ToList();
            if (block[0] == ".") continue;
            for (var j = 0; j < i; j++)
            {
                var spaceBlock = blocks[j];
                if (spaceBlock[0] != ".") continue;
                if (spaceBlock.Count < block.Count) continue;
                blocks[i] = Enumerable.Repeat(".", block.Count).ToList();
                blocks[j] = block;
                var diff = spaceBlock.Count - block.Count;
                if (diff > 0)
                {
                    blocks.Insert(j + 1, spaceBlock[..diff]);
                    i++;
                }

                break;
            }
        }

        Console.WriteLine(blocks.SelectMany(b => b)
            .Select((x, i) => x == "." ? 0 : BigInteger.Parse(x) * new BigInteger(i))
            .Aggregate(new BigInteger(0), (acc, curr) => acc + curr));
    }
}