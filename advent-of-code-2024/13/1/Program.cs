namespace advent_of_code_2024._13._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var lines = File.ReadAllLines(path);
        var totalCost = 0;
        for (var i = 0; i < lines.Length; i += 4)
        {
            var buttonAArgs = lines[i][12..].Split(", Y+");
            var buttonA = new Button(int.Parse(buttonAArgs[0]), int.Parse(buttonAArgs[1]));
            var buttonBArgs = lines[i+1][12..].Split(", Y+");
            var buttonB = new Button(int.Parse(buttonBArgs[0]), int.Parse(buttonBArgs[1]));
            var prizeArgs = lines[i+2][9..].Split(", Y=");
            var prize = new Prize(int.Parse(prizeArgs[0]), int.Parse(prizeArgs[1]));

            var minCost = int.MaxValue;
            var found = false;

            for (var cA = 0; cA < 101; cA++)
            {
                for (var cB = 0; cB < 101; cB++)
                {
                    var x = cA*buttonA.Xd + cB*buttonB.Xd;
                    var y = cA*buttonA.Yd + cB*buttonB.Yd;
                    if (x != prize.X || y != prize.Y) continue;
                    found = true;
                    var cost = cA * 3 + cB;
                    if (cost < minCost) minCost = cost;
                }
            }

            if (found)
            {
                totalCost += minCost;
            }
        }
        
        Console.WriteLine(totalCost);
    }

    private record Prize(int X, int Y);

    private record Button(int Xd, int Yd);
}


// Button A: X+94, Y+34
// Button B: X+22, Y+67
// Prize: X=8400, Y=5400

// what is the upper bound of A pushes?
// X: floor(8400 / 94) = 89
// Y: floor(5400 / 34) = 158
// min(floor(8400 / 94),floor(5400 / 34)) = 89
// where would that be?
// x = 
// y = 

// what is the upper bound of B pushes?
// X: floor(8400 / 22) = 381
// Y: floor(5400 / 67) = 80
// min(floor(8400 / 22),floor(5400 / 67)) = 80
// where would that be?
// x = 80 * 22 = 1760
// y = 80 * 67 = 5360