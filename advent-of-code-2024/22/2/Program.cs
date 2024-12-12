using System.Numerics;

namespace advent_of_code_2024._22._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var initialSecrets = File.ReadAllLines(path).Select(BigInteger.Parse).ToList();
        var lookup = new Dictionary<string, BigInteger>();
        var sum = new BigInteger(0);

        foreach (var initialSecret in initialSecrets)
        {
            var secret = initialSecret;
            var buyer = new List<PriceItem>();
            var price = secret % 10;
            buyer.Add(new PriceItem(price, null));
            var prev = price;
            for (var i = 0; i < 1999; i++)
            {
                secret = Prng(secret);
                var p = secret % 10;
                buyer.Add(new PriceItem(p, p - prev));
                prev = p;
            }

            var blookup = new Dictionary<string, BigInteger>();
            for (var i = 3; i < buyer.Count; i++)
            {
                var key = buyer[(i - 3)..(i + 1)].Aggregate("", (a, c) => a + "," + c.Change);
                if (blookup.ContainsKey(key)) continue;
                var p = buyer[i].Price;
                blookup.Add(key, p);

                if (lookup.TryGetValue(key, out var value))
                    lookup[key] = value + p;
                else
                    lookup.Add(key, p);

                sum = BigInteger.Max(sum, lookup[key]);
            }
        }

        Console.WriteLine(sum);
        Console.WriteLine(sum == 2042);
    }

    private static BigInteger Prng(BigInteger n)
    {
        var s = Prune(Mix(n, n * 64));
        s = Prune(Mix(s, BigInteger.Divide(s, 32)));
        return Prune(Mix(s, s * 2048));
    }

    private static BigInteger Mix(BigInteger n, BigInteger m) => n ^ m;

    private static BigInteger Prune(BigInteger n) => n % 16777216;

    private record PriceItem(BigInteger Price, BigInteger? Change);
}