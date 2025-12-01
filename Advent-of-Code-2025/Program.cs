using System.Diagnostics;

namespace Advent_of_Code_2025;

class Program
{
    static void Main(string[] args)
    {
        // run todays method
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Day1();
        stopwatch.Stop();
        Console.WriteLine("-----------------------------------\n" +
                          "Runtime: " + stopwatch.Elapsed);
    }

    static string[] ReadInput(int dayNumber)
    {
        return File.ReadAllLines(@"C:\Users\Fezum\RiderProjects\Advent-of-Code-2025\Advent-of-Code-2025\Input\Day" + dayNumber + ".txt");
    }

    static void Day1()
    {
        var lines = ReadInput(1);

        var value = 50;
        var amount0 = 0;
        var amount0_part2 = 0;
        foreach (var line in lines)
        {
            var number = int.Parse(line[1..]);
            var goesUp = line.StartsWith('R');
            for (var i = 0; i < number; i++)
            {
                value += goesUp ? 1 : -1;
                value %= 100;
                if (value == 0) amount0_part2++;
            }
            if (value == 0) amount0++;
        }

        Console.WriteLine(amount0);
        Console.WriteLine("Part 2: {0}", amount0_part2);
    }
}