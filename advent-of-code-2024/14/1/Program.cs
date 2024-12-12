namespace advent_of_code_2024._14._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var width = 101;
        // var width = 11;
        var height = 103;
        // var height = 7;
        var t = 100;
        var lines = File.ReadAllLines(path);

        var q1 = 0;
        var q2 = 0;
        var q3 = 0;
        var q4 = 0;

        foreach (var l in lines)
        {
            var parts = l.Split(" ");
            var p = parts[0][2..].Split(",");
            var v = parts[1][2..].Split(",");
            var px = int.Parse(p[0]);
            var vx = int.Parse(v[0]);
            var py = int.Parse(p[1]);
            var vy = int.Parse(v[1]);

            var sx = PosMod(px + t * vx, width);
            var sy = PosMod(py + t * vy, height);

            var cx = Math.DivRem(width, 2).Quotient;
            var cy = Math.DivRem(height, 2).Quotient;
            if (sx < cx)
            {
                if (sy < cy) q1++;
                else if (sy > cy) q3++;
            }
            else if (sx > cx)
            {
                if (sy < cy) q2++;
                else if (sy > cy) q4++;
            }
        }

        Console.WriteLine(q1 * q2 * q3 * q4);
    }

    private static int PosMod(int x, int m)
    {
        return (x % m + m) % m;
    }
}