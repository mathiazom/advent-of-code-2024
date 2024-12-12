using System.Diagnostics;

namespace advent_of_code_2024;

public class Program
{
    public static void Main(string[] args)
    {
        var day = int.Parse(args[0]);
        var part = int.Parse(args[1]);
        var file = args[2];
        var type = Type.GetType($"{typeof(Program).Namespace}._{day}._{part}.Program");
        if (type == null)
            throw new TypeLoadException("Program not found");
        var obj = Activator.CreateInstance(type);
        if (obj == null)
            throw new MissingMethodException("Program could not be instantiated");
        if (!type.GetInterfaces().Contains(typeof(ISolution)))
            throw new MissingMethodException($"Program not implementing {nameof(ISolution)}");
        var sw = Stopwatch.StartNew();
        ((ISolution) obj).Run($"{day}/{part}/" + file);
        Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
    }
}