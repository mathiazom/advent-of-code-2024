namespace advent_of_code_2024._25._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);

        List<int[]> locks = [];
        List<int[]> keys = [];
        
        for (var i = 0; i < lines.Length; i += 8)
        {
            int[] heights = [0, 0, 0, 0, 0];
            for (var j = 0; j < 5; j++)
            {
                for (var k = 0; k < 5; k++)
                {
                    heights[k] += lines[i+j+1][k] == '#' ? 1 : 0;
                }
            }
            if (lines[i] == "#####") locks.Add(heights);
            else keys.Add(heights);
        }

        var count = 0;
        foreach (var l in locks)
        {
            foreach (var key in keys)
            {
                var ok = true;
                for (var i = 0; i < 5; i++)
                {
                    if (l[i] + key[i] <= 5) continue;
                    ok = false;
                    break;
                }
                if (ok) count++;
            }
        }
        
        Console.WriteLine(count);
        Console.WriteLine(count == 3284);
    }
}