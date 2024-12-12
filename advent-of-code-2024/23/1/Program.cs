namespace advent_of_code_2024._23._1;

public class Program : ISolution
{
    public void Run(string path)
    {
        var connections = File.ReadAllLines(path).Select(l => l.Split("-")).ToList();
        var computers = new Dictionary<string, Computer>();

        foreach (var connection in connections)
        {
            foreach (var c in connection)
            {
                var friends = connection.Where(l => l != c).ToList();
                if (!computers.TryGetValue(c, out var computer))
                {
                    computers.Add(c, new Computer(c, friends));
                    continue;
                }

                computer.Friends.AddRange(friends.Where(f => !computer.Friends.Contains(f)));
            }
        }

        var count = (from computer in computers.Values
                where computer.Name.StartsWith('t')
                from first in computer.Friends
                from second in computer.Friends
                where first != second
                where computers[first].Friends.Contains(second)
                select new[] { first, second, computer.Name }.Order())
            .Select(t => string.Join(',', t))
            .Distinct().Count();

        Console.WriteLine(count);
        Console.WriteLine(count == 1419);
    }

    private record Computer(string Name, List<string> Friends);
}