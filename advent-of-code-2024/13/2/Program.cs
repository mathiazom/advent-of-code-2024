using System.Numerics;

namespace advent_of_code_2024._13._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        BigInteger totalCost = 0;
        for (var i = 0; i < lines.Length; i += 4)
        {
            var aArgs = lines[i][12..].Split(", Y+");
            var a = new BigVec(BigInteger.Parse(aArgs[0]), BigInteger.Parse(aArgs[1]));
            var bArgs = lines[i + 1][12..].Split(", Y+");
            var b = new BigVec(BigInteger.Parse(bArgs[0]), BigInteger.Parse(bArgs[1]));
            var pArgs = lines[i + 2][9..].Split(", Y=");
            var offset = new BigInteger(10000000000000);
            var p = new BigVec(BigInteger.Parse(pArgs[0]) + offset, BigInteger.Parse(pArgs[1]) + offset);

            var cA = BigInteger.DivRem(p.X * b.Y - b.X * p.Y, a.X * b.Y - b.X * a.Y, out var rA);
            var cB = BigInteger.DivRem(-p.X * a.Y + a.X * p.Y, a.X * b.Y - b.X * a.Y, out var rB);

            if (rA == 0 && rB == 0) totalCost += cA * 3 + cB;
        }

        Console.WriteLine(totalCost);
        Console.WriteLine(totalCost == 72587986598368);
    }

    private record BigVec(BigInteger X, BigInteger Y);
}