namespace advent_of_code_2024._7._2;

using System.Numerics;


public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        BigInteger sum = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(": ");
            var res = BigInteger.Parse(parts[0]);
            var nums = parts[1].Split(" ").Select(BigInteger.Parse).ToList();
            List<List<Operation>> permutations = [];
            // Console.WriteLine(res);
            // Console.WriteLine(nums.Count);
            for (int i = 0; i < Math.Pow(3, (nums.Count - 1)); i++)
            {
                Console.WriteLine(BigIntToBase3(i).PadLeft(nums.Count-1, '0'));
                permutations.Add(BigIntToBase3(i).PadLeft(nums.Count-1, '0').Select(s => s.Equals('0') ? Operation.Add : s.Equals('1') ? Operation.Mul : Operation.Cat).ToList());
            }
            // Console.WriteLine(string.Join("\n", permutations.Select(p => string.Join(" ", p))));
            foreach (var operations in permutations)
            {
                // Console.WriteLine(string.Join(" ", operations));
                var opNums = nums.Skip(1).Select((num, index) => new { num, op = operations[index] });
                var test = opNums.Aggregate(nums[0], (acc, curr) =>
                {
                    return curr.op switch
                    {
                        Operation.Add => acc + curr.num,
                        Operation.Mul => acc * curr.num,
                        _ => BigInteger.Parse(acc.ToString() + curr.num)
                    };
                });
                if (test == res)
                {
                    Console.WriteLine(res);
                    sum += res;
                    break;
                }
            }

        }
        Console.WriteLine(sum);

        string BigIntToBase3(BigInteger value)
        {
            string result = string.Empty;
            do
            {
                result = "0123456789ABCDEF"[(int)(value % 3)] + result;
                value /= 3;
            }
            while (value > 0);

            return result;
        }
    }
}

internal enum Operation { Add, Mul, Cat };