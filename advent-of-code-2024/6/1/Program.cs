namespace advent_of_code_2024._6._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path).ToList();
        var count = 0;
        var posX = 0;
        var posY = 0;
        
        for (var i = 0; i < lines.Count; i++)
        {
            for (var j = 0; j < lines[0].Length; j++)
            {
                var cell = lines[i][j];
                if (cell != '^') continue;
                posY = i;
                posX = j;
                var newLine = lines[posY].ToArray();
                newLine[posX] = 'X';
                lines[posY] = new string(newLine);
                count++;
            }
        }
        
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
            switch (nextCell)
            {
                case '.' or 'X':
                {
                    if (nextCell == '.')
                    {
                        var newLine = lines[nextY].ToArray();
                        newLine[nextX] = 'X';
                        lines[nextY] = new string(newLine);
                        count++;
                    }
                    posY = nextY;
                    posX = nextX;
                    break;
                }
                case '#':
                {
                    switch (dirX)
                    {
                        case 1 when dirY == 0:
                            dirX = 0;
                            dirY = 1;
                            break;
                        case 0 when dirY == 1:
                            dirX = -1;
                            dirY = 0;
                            break;
                        case -1 when dirY == 0:
                            dirX = 0;
                            dirY = -1;
                            break;
                        case 0 when dirY == -1:
                            dirX = 1;
                            dirY = 0;
                            break;
                    }
                    break;
                }
            }
        }
        
        Console.WriteLine(count);
    }
}