namespace advent_of_code_2024._12._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        var inc = 0;
        var plots = lines.Select(l => l.Select(c => new Plot { Type = c, RegionId = inc++ }).ToList()).ToList();
        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines.Length; j++)
            {
                var plot = plots[i][j];
                var perimeter = 0;
                var top = i == 0 ? null : plots[i - 1][j];
                var left = j == 0 ? null : plots[i][j - 1];
                var bottom = i == lines.Length - 1 ? null : plots[i + 1][j];
                var right = j == lines.Length - 1 ? null : plots[i][j + 1];
                List<Plot?> adj = [top, right, bottom, left];
                List<int> regionsToMerge = [];
                foreach (var a in adj)
                {
                    if (a == null || a.Type != plot.Type)
                    {
                        perimeter++;
                        continue;
                    }

                    regionsToMerge.Add(a.RegionId);
                }

                plot.Perimeter = perimeter;

                if (regionsToMerge.Count == 0) continue;

                for (var k = 0; k < plots.Count; k++)
                {
                    for (var l = 0; l < plots.Count; l++)
                    {
                        var toMerge = plots[k][l];
                        if (regionsToMerge.Contains(toMerge.RegionId))
                        {
                            toMerge.RegionId = plot.RegionId;
                        }
                    }
                }
            }
        }

        var sum = plots.SelectMany(p => p).GroupBy(p => p.RegionId).Aggregate(0,
            (acc, g) => acc + g.Count() * g.Aggregate(0, (pacc, p) => pacc + p.Perimeter));
        Console.WriteLine(sum);
        Console.WriteLine(sum == 1533644);
    }

    private class Plot
    {
        public char Type { get; set; }
        public int RegionId { get; set; }
        public int Perimeter { get; set; }
    }
}