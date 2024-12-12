namespace advent_of_code_2024._6._2;

public class Program : ISolution
{
    public void Run(string path)
    {
        var baseLines = File.ReadAllLines(path).ToList();
        var startPosX = 0;
        var startPosY = 0;

        for (var i = 0; i < baseLines.Count; i++)
        {
            for (var j = 0; j < baseLines[0].Length; j++)
            {
                if (baseLines[i][j] != '^') continue;
                startPosY = i;
                startPosX = j;
                var newLine = baseLines[i].ToArray();
                newLine[j] = '.';
                baseLines[i] = new string(newLine);
            }
        }


        var count = 0;


        for (var i = 0; i < baseLines.Count; i++)
        {
            for (var j = 0; j < baseLines[0].Length; j++)
            {
                var lines = baseLines.ToList();
                var newLine = lines[i].ToArray();
                newLine[j] = '#';
                lines[i] = new string(newLine);

                var visits = new List<List<List<Direction>>>();
                for (int k = 0; k < baseLines.Count; k++)
                {
                    var list = new List<List<Direction>>();
                    for (int l = 0; l < baseLines[0].Length; l++)
                    {
                        list.Add(new List<Direction>());
                    }

                    visits.Add(list);
                }

                var posX = startPosX;
                var posY = startPosY;

                var dirX = 0;
                var dirY = -1;

                while (true)
                {
                    var nextY = posY + dirY;
                    var nextX = posX + dirX;
                    if (nextX < 0 || nextX >= lines[0].Length || nextY < 0 || nextY >= lines[0].Length)
                    {
                        break;
                    }

                    var nextCell = lines[nextY][nextX];
                    if (nextCell == '.')
                    {
                        posY = nextY;
                        posX = nextX;
                        Direction direction;
                        if (dirY == 1)
                        {
                            direction = Direction.Down;
                        }
                        else if (dirY == -1)
                        {
                            direction = Direction.Up;
                        }
                        else if (dirX == 1)
                        {
                            direction = Direction.Forward;
                        }
                        else
                        {
                            direction = Direction.Back;
                        }

                        var cellVisits = visits[posX][posY];
                        if (cellVisits.Contains(direction))
                        {
                            count++;
                            break;
                        }

                        cellVisits.Add(direction);
                        visits[posX][posY] = cellVisits;
                        continue;
                    }

                    if (nextCell == '#')
                    {
                        if (dirX == 1 && dirY == 0)
                        {
                            dirX = 0;
                            dirY = 1;
                        }
                        else if (dirX == 0 && dirY == 1)
                        {
                            dirX = -1;
                            dirY = 0;
                        }
                        else if (dirX == -1 && dirY == 0)
                        {
                            dirX = 0;
                            dirY = -1;
                        }
                        else if (dirX == 0 && dirY == -1)
                        {
                            dirX = 1;
                            dirY = 0;
                        }
                    }
                }
            }
        }

        Console.WriteLine(count);
    }
}


internal enum Direction
{
    Up,
    Down,
    Back,
    Forward
}