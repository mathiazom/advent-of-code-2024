namespace advent_of_code_2024._14._2;

public class Program : ISolution
{
    static int width = 101;

    // static int width = 11;
    static int height = 103;
    // static int height = 7;

    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);

        var robots = new List<Robot>();
        foreach (var l in lines)
        {
            var parts = l.Split(" ");
            var p = parts[0][2..].Split(",");
            var v = parts[1][2..].Split(",");
            robots.Add(new Robot
                { Px = int.Parse(p[0]), Py = int.Parse(p[1]), Vx = int.Parse(v[0]), Vy = int.Parse(v[1]) });
        }

        var startT = 6491;
        var stopT = 6493;
        
        for (var t = 0; t < stopT; t++)
        {
            var map = new List<List<char>>();
            for (var i = 0; i < height; i++)
            {
                map.Add(Enumerable.Repeat(' ', width).ToList());
            }
            foreach (var robot in robots)
            {
                robot.Tick();
                map[robot.Py][robot.Px] = '\u25a0';
            }
            
            if (t < startT) continue;
            Thread.Sleep(200);
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            for (int i = 0; i < map.Count-1; i+=2)
            {
                Console.WriteLine(string.Join("", map[i]) + "|" + string.Join("", map[i+1]));
            }
            Console.WriteLine(t);
        }
    }

    private static int PosMod(int x, int m)
    {
        return (x % m + m) % m;
    }

    private class Robot()
    {
        public int Px { get; set; }
        public int Py { get; set; }
        public int Vx { get; init; }
        public int Vy { get; init; }

        public void Tick()
        {
            Px = PosMod(Px + Vx, width);
            Py = PosMod(Py + Vy, height);
        }
    }
}