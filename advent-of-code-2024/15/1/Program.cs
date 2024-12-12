namespace advent_of_code_2024._15._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        var map = new List<List<char>>();
        var moves = string.Empty;
        var y = 0;
        var x = 0;
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var ml = line.ToCharArray().ToList();
            for (var j = 0; j < ml.Count; j++)
            {
                if (ml[j] != '@') continue;
                y = i;
                x = j;
                break;
            }
            map.Add(ml);
            if (line.Trim().Length != 0) continue;
            moves = string.Join("", lines[(i + 1)..]);
            break;
        }
        foreach (var move in moves)
        {
            var yd = move switch
            {
                '^' => -1,
                'v' => 1,
                _ => 0
            };
            var xd = move switch
            {
                '<' => -1,
                '>' => 1,
                _ => 0
            };
            var next = map[y+yd][x+xd];
            switch (next)
            {
                case '#':
                    continue;
                case '.':
                    map[y][x] = '.';
                    y += yd;
                    x += xd;
                    map[y][x] = '@';
                    break;
                case 'O':
                    for (var i = 2; i < lines.Length; i++)
                    {
                        var n = map[y+yd*i][x+xd*i];
                        if (n == '#') break;
                        if (n != '.') continue;
                        map[y+yd*i][x+xd*i] = 'O';
                        map[y][x] = '.';
                        y += yd;
                        x += xd;
                        map[y][x] = '@';
                        break;
                    }
                    break;
            }
        }
        
        var sum = 0;
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == 'O')
                {
                    sum += i * 100 + j;
                }
            }
        }
        Console.WriteLine(sum);
        Console.WriteLine(sum == 1294459);
    }
}