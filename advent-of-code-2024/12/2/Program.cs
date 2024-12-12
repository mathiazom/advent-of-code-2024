namespace advent_of_code_2024._12._2;

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
                var top = i == 0 ? null : plots[i - 1][j];
                List<int> regionsToMerge = [];
                if (top == null || top.Type != plot.Type)
                    plot.PTop = true;
                else
                    regionsToMerge.Add(top.RegionId);
                var right = j == lines.Length - 1 ? null : plots[i][j + 1];
                if (right == null || right.Type != plot.Type)
                    plot.PRight = true;
                else
                    regionsToMerge.Add(right.RegionId);
                var bottom = i == lines.Length - 1 ? null : plots[i + 1][j];
                if (bottom == null || bottom.Type != plot.Type)
                    plot.PBottom = true;
                else
                    regionsToMerge.Add(bottom.RegionId);
                var left = j == 0 ? null : plots[i][j - 1];
                if (left == null || left.Type != plot.Type)
                    plot.PLeft = true;
                else
                    regionsToMerge.Add(left.RegionId);

                if (regionsToMerge.Count == 0) continue;

                foreach (var toMerge in plots.SelectMany(plotLine => plotLine.Where(toMerge => regionsToMerge.Contains(toMerge.RegionId))))
                {
                    toMerge.RegionId = plot.RegionId;
                }
            }
        }

        for (var i = 0; i < lines.Length; i++)
        {
            for (var j = 0; j < lines.Length; j++)
            {
                var plot = plots[i][j];
                var left = j == 0 ? null : plots[i][j - 1];

                if (plot.PTop && (left == null || !left.PTop || left.Type != plot.Type))
                {
                    plot.Sides++;
                }
                
                var top = i == 0 ? null : plots[i - 1][j];

                if (plot.PRight && (top == null || !top.PRight || top.Type != plot.Type))
                {
                    plot.Sides++;
                }
                var right = j == lines.Length - 1 ? null : plots[i][j + 1];

                if (plot.PBottom && (right == null || !right.PBottom || right.Type != plot.Type))
                {
                    plot.Sides++;
                }
                
                var bottom = i == lines.Length - 1 ? null : plots[i + 1][j];

                if (plot.PLeft && (bottom == null || !bottom.PLeft || bottom.Type != plot.Type))
                {
                    plot.Sides++;
                }
            }
        }

        var sum = plots.SelectMany(p => p).GroupBy(p => p.RegionId).Aggregate(0,
            (acc, g) => acc + g.Count() * g.Aggregate(0, (pacc, p) => pacc + p.Sides));
        Console.WriteLine(sum);
        Console.WriteLine(sum == 936718);
    }

    private class Plot
    {
        public char Type { get; init; }
        public int RegionId { get; set; }
        public bool PTop { get; set; }
        public bool PRight { get; set; }
        public bool PBottom { get; set; }
        public bool PLeft { get; set; }
        
        public int Sides { get; set; }
        
    }
}