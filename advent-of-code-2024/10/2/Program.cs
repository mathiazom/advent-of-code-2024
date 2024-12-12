namespace advent_of_code_2024._10._2;

public class Program : ISolution
{

    public void Run(string path)
    {
        var sum = 0;
        var lines = File.ReadAllLines(path);
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                var cell = int.Parse(lines[i][j].ToString());
                if (cell != 0) continue;
                sum += TrailScore(lines, j, i, cell);
            }
        }
        Console.WriteLine(sum);
    }

    private static int TrailScore(string[] lines, int posX, int posY, int height)
    {
        if (height == 9)
        {
            return 1;
        }

        var sum = 0;
        if (posX + 1 < lines.Length)
        {
            var cell1 = int.Parse(lines[posY][posX+1].ToString());
            if (cell1 == height + 1)
            {
                sum += TrailScore(lines, posX + 1, posY, cell1);
            }
        }

        if (posY + 1 < lines.Length)
        {
            var cell2 = int.Parse(lines[posY+1][posX].ToString());
            if (cell2 == height + 1)
            {
                sum += TrailScore(lines, posX, posY + 1, cell2);
            }
        }

        if (posX - 1 >= 0)
        {
            var cell3 = int.Parse(lines[posY][posX-1].ToString());
            if (cell3 == height + 1)
            {
                sum += TrailScore(lines, posX - 1, posY, cell3);
            }
        }

        if (posY - 1 >= 0)
        {
            var cell4 = int.Parse(lines[posY-1][posX].ToString());
            if (cell4 == height + 1)
            {
                sum += TrailScore(lines, posX, posY - 1, cell4);
            }
        }

        return sum;
    }
}

internal record Pos(int X, int Y)
{
    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}