namespace advent_of_code_2024._2._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var count = 0;
        foreach (var line in File.ReadAllLines(path))
        {
            var levels = line.Split(" ").Select(int.Parse).ToArray();
            if (IsSafe(levels))
            {
                count++;
                continue;
            }

            var safe = false;
            for (var i = 0; i < levels.Length; i++)
            {
                var dampened = levels.ToList();
                dampened.RemoveAt(i);
                if (!IsSafe(dampened.ToArray())) continue;
                safe = true;
                break;
            }

            if (safe) count++;
        }

        Console.WriteLine(count);
    }

    private static bool IsSafe(int[] levels)
    {
        var previous = levels[0];
        var direction = 0;
        for (var i = 1; i < levels.Length; i++)
        {
            var diff = levels[i] - previous;

            if (diff == 0)
                return false;

            if (direction == 0) direction = Math.Sign(diff);

            if (Math.Sign(diff) != direction && direction != 0)
                return false;

            if (diff is < -3 or > 3)
                return false;

            previous = levels[i];
        }

        return true;
    }
}