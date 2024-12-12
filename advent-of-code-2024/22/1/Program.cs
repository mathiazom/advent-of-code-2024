using System.Numerics;

namespace advent_of_code_2024._22._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        BigInteger sum = 0;
        var initialSecrets = File.ReadAllLines(path).Select(BigInteger.Parse).ToList();

        foreach (var initialSecret in initialSecrets)
        {
            var secret = initialSecret;
            for (var i = 0; i < 2000; i++) secret = Prng(secret);

            sum += secret;
        }

        Console.WriteLine(sum);
        Console.WriteLine(sum == 17960270302);
    }

    private static BigInteger Prng(BigInteger n)
    {
        var s = Prune(Mix(n, n * 64));
        s = Prune(Mix(s, BigInteger.Divide(s, 32)));
        return Prune(Mix(s, s * 2048));
    }

    private static BigInteger Mix(BigInteger n, BigInteger m) => n ^ m;

    private static BigInteger Prune(BigInteger n) => n % 16777216;
}