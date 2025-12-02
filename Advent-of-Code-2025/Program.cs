using System.Diagnostics;

namespace Advent_of_Code_2025;

class Program
{
    static void Main(string[] args)
    {
        // run todays method
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Day2(true);
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
}