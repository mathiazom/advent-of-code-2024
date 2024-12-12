using System.Diagnostics.CodeAnalysis;

namespace advent_of_code_2024._15._2;

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
            var line = lines[i].Replace("O", "[]").Replace(".", "..").Replace("#", "##").Replace("@", "@.");
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
            // Thread.Sleep(100);
            // Console.Clear();
            // Console.WriteLine("\x1b[3J");
            // Console.WriteLine(string.Join("\n", map.Select(m => string.Join("", m))));
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
            var next = map[y + yd][x + xd];
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
                case '[':
                case ']':
                    var lx = next == '[' ? x + xd : x + xd - 1;
                    var dir = new Vec2(xd, yd);
                    if (!IsClear(map, new Vec2(lx, y + yd), dir, out var boxes)) break;
                    foreach (var box in boxes)
                    {
                        map[box.Y][box.X] = '.';
                        map[box.Y][box.X + 1] = '.';
                    }

                    foreach (var box in boxes)
                    {
                        map[box.Y + dir.Y][box.X + dir.X] = '[';
                        map[box.Y + dir.Y][box.X + dir.X + 1] = ']';
                    }

                    map[y][x] = '.';
                    x += xd;
                    y += yd;
                    map[y][x] = '@';
                    break;
            }
        }

        var sum = 0;
        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == '[')
                {
                    sum += i * 100 + j;
                }
            }
        }

        Console.WriteLine(sum);
        Console.WriteLine(sum == 1319212);
    }


    private static bool IsClear(List<List<char>> map, Vec2 left, Vec2 dir, [MaybeNullWhen(false)] out List<Vec2> boxes)
    {
        var nextLeft = map[left.Y + dir.Y][left.X + 2 * dir.X];
        var nextRight = map[left.Y + dir.Y][left.X + 2 * dir.X + 1];

        if ((dir.X == 1 && nextLeft == '.') || (dir.X == -1 && nextRight == '.'))
        {
            boxes = [left];
            return true;
        }

        if (nextLeft == '#' || nextRight == '#')
        {
            boxes = null;
            return false;
        }

        if (nextLeft == '.' && nextRight == '.')
        {
            boxes = [left];
            return true;
        }

        if (nextLeft == '[')
        {
            if (!IsClear(map, new Vec2(left.X + 2 * dir.X, left.Y + dir.Y), dir, out boxes)) return false;
            boxes.Add(left);
            return true;
        }

        List<Vec2> newBoxes = [left];
        if (nextLeft == ']')
        {
            if (!IsClear(map, new Vec2(left.X + 2 * dir.X - 1, left.Y + dir.Y), dir, out boxes))
            {
                return false;
            }

            newBoxes.AddRange(boxes);
        }

        if (nextRight == '[')
        {
            if (!IsClear(map, new Vec2(left.X + 2 * dir.X + 1, left.Y + dir.Y), dir, out boxes))
            {
                return false;
            }

            newBoxes.AddRange(boxes);
        }

        boxes = newBoxes;
        return true;
    }

    private record Vec2(int X, int Y);
}