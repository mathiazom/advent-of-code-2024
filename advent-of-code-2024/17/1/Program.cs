namespace advent_of_code_2024._17._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);

        List<int> outputs = [];

        var a = int.Parse(lines[0].Split(" ")[^1]);
        var b = int.Parse(lines[1].Split(" ")[^1]);
        var c = int.Parse(lines[2].Split(" ")[^1]);
        var program = lines[^1][8..].Split(",").Select(int.Parse).ToArray();

        Console.WriteLine($"A: {a}");
        Console.WriteLine($"B: {b}");
        Console.WriteLine($"C: {c}");
        Console.WriteLine($"Program: {string.Join(",", program)}");

        for (var i = 0; i < program.Length; i += 2)
        {
            var opcode = program[i];
            var operand = program[i + 1];
            switch (opcode)
            {
                // adv
                case 0:
                    a = Math.DivRem(a, (int)Math.Pow(2, Combo(operand, a, b, c))).Quotient;
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
                    outputs.Add(Combo(operand, a, b, c) % 8);
                    break;
                // bdv
                case 6:
                    b = Math.DivRem(a, (int)Math.Pow(2, Combo(operand, a, b, c))).Quotient;
                    break;
                // cdv
                case 7:
                    c = Math.DivRem(a, (int)Math.Pow(2, Combo(operand, a, b, c))).Quotient;
                    break;
            }
        }

        var output = string.Join(",", outputs);
        Console.WriteLine(output);
        Console.WriteLine(output == "2,7,2,5,1,2,7,3,7");
    }

    private static int Combo(int x, int a, int b, int c)
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