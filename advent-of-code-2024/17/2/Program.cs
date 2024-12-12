using System.Numerics;

namespace advent_of_code_2024._17._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var program = File.ReadAllLines(path)[4][9..];
        var nums = program.Split(",").Select(int.Parse).ToList();

        List<List<long>> sols = [];
        sols.AddRange(nums.Select(_ => (List<long>) []));

        Solve(0, nums, 0, sols);

        List<long> solutions = [];

        foreach (var sol0 in sols[0])
        {
            foreach (var sol1 in sols[1])
            {
                foreach (var sol2 in sols[2])
                {
                    foreach (var sol3 in sols[3])
                    {
                        foreach (var sol4 in sols[4])
                        {
                            foreach (var sol5 in sols[5])
                            {
                                foreach (var sol6 in sols[6])
                                {
                                    foreach (var sol7 in sols[7])
                                    {
                                        foreach (var sol8 in sols[8])
                                        {
                                            foreach (var sol9 in sols[9])
                                            {
                                                foreach (var sol10 in sols[10])
                                                {
                                                    foreach (var sol11 in sols[11])
                                                    {
                                                        foreach (var sol12 in sols[12])
                                                        {
                                                            foreach (var sol13 in sols[13])
                                                            {
                                                                foreach (var sol14 in sols[14])
                                                                {
                                                                    foreach (var sol15 in sols[15])
                                                                    {
                                                                        var sol = 
                                                                            sol15 * (long)Math.Pow(2, 3 * 15) +
                                                                            sol14 * (long)Math.Pow(2, 3 * 14) +
                                                                            sol13 * (long)Math.Pow(2, 3 * 13) +
                                                                            sol12 * (long)Math.Pow(2, 3 * 12) +
                                                                            sol11 * (long)Math.Pow(2, 3 * 11) +
                                                                            sol10 * (long)Math.Pow(2, 3 * 10) +
                                                                            sol9 * (long)Math.Pow(2, 3 * 9) +
                                                                            sol8 * (long)Math.Pow(2, 3 * 8) +
                                                                            sol7 * (long)Math.Pow(2, 3 * 7) +
                                                                            sol6 * (long)Math.Pow(2, 3 * 6) +
                                                                            sol5 * (long)Math.Pow(2, 3 * 5) +
                                                                            sol4 * (long)Math.Pow(2, 3 * 4) +
                                                                            sol3 * (long)Math.Pow(2, 3 * 3) +
                                                                            sol2 * (long)Math.Pow(2, 3 * 2) +
                                                                            sol1 * (long)Math.Pow(2, 3 * 1) +
                                                                            sol0;
                                                                        if (string.Join(",", Test(sol)) == program)
                                                                        {
                                                                            solutions.Add(sol);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        var a = solutions.Min();

        Console.WriteLine(a);
        Console.WriteLine(a == 247839002892474);
    }

    private void Solve(int i, List<int> nums, long a, List<List<long>> sols)
    {
        var num = nums[^(i + 1)];
        for (var b = 0; b < 8; b++)
        {
            var b2 = b ^ 1;
            var m = a ^ b;
            var check = (b2 ^ (m / (long)Math.Pow(2, b2)) ^ 6) % 8;
            if (check != num) continue;
            sols[^(i + 1)].Add(b);
            if (i == nums.Count - 1) continue;

            Solve(i + 1, nums, m * 8, sols);
        }
    }

    private static List<int> Test(BigInteger a)
    {
        List<int> outputs = [];

        BigInteger b = 0;
        BigInteger c = 0;
        int[] program = [2, 4, 1, 1, 7, 5, 0, 3, 4, 7, 1, 6, 5, 5, 3, 0];

        for (var i = 0; i < program.Length; i += 2)
        {
            var opcode = program[i];
            var operand = program[i + 1];
            switch (opcode)
            {
                // adv
                case 0:
                    a = BigInteger.Divide(a, BigInteger.Pow(2, (int)Combo(operand, a, b, c)));
                    break;
                // bxl
                case 1:
                    b ^= operand;
                    break;
                // bst
                case 2:
                    b = Combo(operand, a, b, c) % 8;
                    break;
                // jnz
                case 3:
                    if (a != 0) i = operand - 2;
                    break;
                // bxc
                case 4:
                    b ^= c;
                    break;
                // out
                case 5:
                    outputs.Add((int)(Combo(operand, a, b, c) % 8));
                    break;
                // bdv
                case 6:
                    b = BigInteger.Divide(a, BigInteger.Pow(2, (int)Combo(operand, a, b, c)));
                    break;
                // cdv
                case 7:
                    var combo = Combo(operand, a, b, c);
                    var pow = BigInteger.Pow(2, (int)combo);
                    c = BigInteger.Divide(a, pow);
                    break;
            }
        }

        return outputs;
    }

    private static BigInteger Combo(int x, BigInteger a, BigInteger b, BigInteger c)
    {
        return x switch
        {
            0 or 1 or 2 or 3 => x,
            4 => a,
            5 => b,
            6 => c,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}