namespace advent_of_code_2024._5._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        var rules = new Dictionary<int, List<int>>();
        var isReadingRules = true;
        var sum = 0;
        
        foreach (var line in lines)
        {
            if (line.Trim().Length == 0)
            {
                isReadingRules = false;
                continue;
            }
        
            if (isReadingRules)
            {
                var parts = line.Split("|");
                var left = int.Parse(parts[0]);
                var right = int.Parse(parts[1]);
                if (rules.TryGetValue(left, out var value))
                    rules[left] = value.Append(right).ToList();
                else
                    rules.Add(left, [right]);
            }
            else
            {
                var parts = line.Split(",").Select(int.Parse).ToList();
                var isFixed = false;
                for (var j = 0; j < parts.Count; j++)
                {
                    var part = parts[j];
                    if (!rules.TryGetValue(part, out var value)) continue;
                    for (var k = 0; k < j; k++)
                    {
                        var checkPart = parts[k];
                        if (!value.Contains(checkPart)) continue;
                        parts.RemoveAt(j);
                        parts.Insert(k, part);
                        isFixed = true;
                        j = 0;
                        break;
                    }
                }
        
                if (isFixed) sum += parts[(parts.Count - 1) / 2];
            }
        }
        
        Console.WriteLine(sum);
    }
}