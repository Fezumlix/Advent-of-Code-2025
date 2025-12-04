using System.Diagnostics;

namespace Advent_of_Code_2025;

class Program
{
    static void Main(string[] args)
    {
        // run todays method
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Day4(true);
        stopwatch.Stop();
        Console.WriteLine("-----------------------------------\n" +
                          "Runtime: " + stopwatch.Elapsed);
    }

    static string[] ReadInput(int dayNumber)
    {
        return File.ReadAllLines(@"C:\Users\Fezum\RiderProjects\Advent-of-Code-2025\Advent-of-Code-2025\Input\Day" +
                                 dayNumber + ".txt");
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

    static void Day2(bool part2 = false)
    {
        var ranges = ReadInput(2)[0]
            .Split(',')
            .Select(r =>
                (
                    long.Parse(r.Split('-')[0]),
                    long.Parse(r.Split('-')[1])
                )
            );

        long sum = 0;
        foreach (var (lower, upper) in ranges)
        {
            for (long i = lower; i <= upper; i++)
            {
                var str = i.ToString();
                if (part2)
                {
                    for (int subpart = 1; subpart <= str.Length / 2; subpart++)
                    {
                        if (str.Length % subpart != 0) continue;
                        var part = str[..subpart];
                        if (str == string.Concat(Enumerable.Repeat(part, str.Length / subpart)))
                        {
                            sum += i;
                            break;
                        }
                    }
                }
                else
                {
                    if (str.Length % 2 != 0) continue;
                    var firstHalf = str[..(str.Length / 2)];

                    if (str.EndsWith(firstHalf))
                        sum += i;
                }
            }
        }

        Console.WriteLine(sum);
    }

    static void Day3()
    {
        var packs = ReadInput(3);
        long sum = 0;
        long sum2 = 0;
        foreach (var pack in packs)
        {
            sum += HighestPossible(pack, 2);
            sum2 += HighestPossible(pack, 12);
        }

        Console.WriteLine(sum);
        Console.WriteLine("Part 2: {0}", sum2);
        return;

        long HighestPossible(string digits, int digitsToGrab)
        {
            long number = 0;
            for (int i = digitsToGrab; i > 0; i--)
            {
                char bestDigit = digits[..^(i - 1)]
                    .Select(d => int.Parse(d.ToString()))
                    .Max()
                    .ToString()
                    .First();
                var index = digits.IndexOf(bestDigit, StringComparison.Ordinal);
                long newAdd = int.Parse(bestDigit.ToString()) * (long)Math.Pow(10, i - 1);
                digits = digits[(index + 1)..];
                number += + newAdd;
            }
            return number;
        }
    }

    static void Day4(bool part2 = false)
    {
        var grid = ReadInput(4).Select(line => line.ToString().Select(c => c == '@').ToArray()).ToArray();

        var amount = 0;

        if (part2)
        {
            var removedLastCycle = true;
            while (removedLastCycle)
            {
                removedLastCycle = false;
                for (var y = 0; y < grid.Length; y++)
                {
                    for (var x = 0; x < grid[0].Length; x++)
                    {
                        if (!grid[y][x]) continue;
                        var blocked = 0;
                        for (var diffX = -1; diffX <= 1; diffX++)
                        {
                            for (var diffY = -1; diffY <= 1; diffY++)
                            {
                                if (x + diffX < 0 ||
                                    x + diffX >= grid[0].Length ||
                                    y + diffY < 0 ||
                                    y + diffY >= grid.Length) continue;
                                if (diffX == 0 && diffY == 0) continue;
                                blocked += grid[y + diffY][x + diffX] ? 1 : 0;
                            }
                        }

                        if (blocked < 4)
                        {
                            amount++;
                            grid[y][x] = false;
                            removedLastCycle = true;
                        }
                    }
                }
            }
        }
        else
        {
            for (var y = 0; y < grid.Length; y++)
            {
                for (var x = 0; x < grid[0].Length; x++)
                {
                    if (!grid[y][x]) continue;
                    var blocked = 0;
                    for (var diffX = -1; diffX <= 1; diffX++)
                    {
                        for (var diffY = -1; diffY <= 1; diffY++)
                        {
                            if (x + diffX < 0 || x + diffX >= grid[0].Length || y + diffY < 0 ||
                                y + diffY >= grid.Length) continue;
                            if (diffX == 0 && diffY == 0) continue;
                            blocked += grid[y + diffY][x + diffX] ? 1 : 0;
                        }
                    }

                    if (blocked < 4) amount++;
                }
            }
        }

        Console.WriteLine(amount);
    }
}