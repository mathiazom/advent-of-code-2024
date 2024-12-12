using System.Numerics;

namespace advent_of_code_2024._9._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        BigInteger checksum = 0;
        var lines = File.ReadAllLines(path);
        var line = lines[0];
        var blocks = new List<string>();
        var fileBlockCount = 0;
        for (var i = 0; i < line.Length; i++)
        {
            var c = int.Parse(line[i].ToString());
            var isFile = i % 2 == 0;
            for (var j = 0; j < c; j++)
            {
                blocks.Add(isFile ? (i / 2).ToString() : ".");
            }

            if (isFile) fileBlockCount += c;
        }

        for (var i = 0; i < fileBlockCount; i++)
        {
            var block = blocks[i];
            if (block == ".")
            {
                while (block == ".")
                {
                    block = blocks.Last();
                    blocks.RemoveAt(blocks.Count - 1);
                }

                blocks[i] = block;
            }

            checksum += i * BigInteger.Parse(block);
        }

        Console.WriteLine(checksum);
    }
}