using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Advent_of_Code_2025;

class Program
{
    static void Main(string[] args)
    {
        // run todays method
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Day8(true);
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
                number += +newAdd;
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

    static void Day5()
    {
        var input = ReadInput(5);

        List<(long, long)> ranges = [];
        int amount = 0;

        foreach (var line in input)
        {
            if (line.Contains('-'))
            {
                ranges.Add((long.Parse(line.Split('-')[0]), long.Parse(line.Split('-')[1])));
            }
            else if (line != "")
            {
                var number = long.Parse(line);
                if (ranges.Any(r => r.Item1 <= number && number <= r.Item2)) amount++;
            }
        }

        Console.WriteLine(amount);

        ranges.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        bool changeDone = true;
        while (changeDone)
        {
            changeDone = false;
            foreach (var (lower, upper) in ranges)
            {
                var otherRanges = ranges.Where(r => r != (lower, upper)).ToList();

                if (otherRanges.Any(r => r.Item1 <= lower && r.Item2 >= upper))
                {
                    ranges.Remove((lower, upper));
                    changeDone = true;
                    break;
                }

                var lowerOverlap = otherRanges.FirstOrDefault(r => lower >= r.Item1 && lower <= r.Item2, (-1, -1));
                var upperOverlap = otherRanges.FirstOrDefault(r => upper >= r.Item1 && upper <= r.Item2, (-1, -1));
                if (lowerOverlap != (-1, -1))
                {
                    ranges.Remove(lowerOverlap);
                    ranges.Remove((lower, upper));
                    changeDone = true;
                    ranges.Add((lowerOverlap.Item1, upper));
                    break;
                }

                if (upperOverlap != (-1, -1))
                {
                    ranges.Remove(upperOverlap);
                    ranges.Remove((lower, upper));
                    changeDone = true;
                    ranges.Add((lower, upperOverlap.Item2));
                    break;
                }
            }
        }

        // ranges.ForEach(r => Console.WriteLine(r.Item1 + "-" + r.Item2));

        var amount2 = 0L;
        foreach (var (lower, upper) in ranges) amount2 += upper - lower + 1;

        Console.WriteLine("Part 2: {0}", amount2);
    }

    static void Day6()
    {
        var input = ReadInput(6);
        List<long[]> numbers = input[..^1]
            .Select(line =>
                Regex.Matches(line, @"\d+")
                    .Select(m => long.Parse(m.Value))
                    .ToArray())
            .ToList();

        bool[] isAddition = Regex.Matches(input[^1], "[+*]").Select(c => c.Value == "+").ToArray();

        long sum = 0;
        for (int i = 0; i < isAddition.Length; i++)
        {
            if (isAddition[i])
            {
                Console.WriteLine(
                    $"{string.Join(" + ", numbers.Select(n => n[i].ToString()))} = {numbers.Select(n => n[i]).Sum()}");
                sum += numbers.Select(n => n[i]).Sum();
            }
            else
            {
                Console.WriteLine(
                    $"{string.Join(" * ", numbers.Select(n => n[i].ToString()))} = {numbers.Select(n => n[i]).Aggregate((a, b) => a * b)}");
                sum += numbers.Select(n => n[i]).Aggregate((a, b) => a * b);
            }
        }

        Console.WriteLine(sum);
    }

    static void Day6Part2()
    {
        var input = ReadInput(6);
        int maxLength = input.Select(line => line.Length).Max();
        for (int i = 0; i < input.Length - 1; i++)
        {
            input[i] = input[i].PadRight(maxLength);
        }

        input[^1] = input[^1].PadRight(maxLength + 1);

        bool[] isAddition = Regex.Matches(input[^1], "[+*]").Select(c => c.Value == "+").ToArray();
        int[] lengths = Regex.Matches(input[^1], @"\s+").Select(str => str.Length).ToArray();

        List<long[]> numbers = [];
        for (int i = 0; i < lengths.Length; i++)
        {
            int startIndex = lengths[..i].Sum() + i;
            int endIndex = startIndex + lengths[i];
            var n = new long[endIndex - startIndex];
            for (int j = startIndex; j < endIndex; j++)
            {
                n[j - startIndex] =
                    long.Parse(input[..^1].Select(line => line[j].ToString()).Aggregate((a, b) => a + b));
                Console.WriteLine(n[j - startIndex]);
            }

            numbers.Add(n);
        }

        long sum = 0;
        for (int i = 0; i < isAddition.Length; i++)
        {
            if (isAddition[i])
            {
                sum += numbers[i].Sum();
            }
            else
            {
                sum += numbers[i].Aggregate((a, b) => a * b);
            }
        }

        Console.WriteLine(sum);

        // 11299263623062
    }

    static void Day7()
    {
        var input = ReadInput(7);
        HashSet<(int, int)> currentBeams =
        [
            (input[0].IndexOf('S'), 0),
        ];

        var splitAmount = 0;
        var currentDepth = 0;

        for (; currentDepth < input.Length; currentDepth++)
        {
            foreach (var beam in currentBeams.Where(b => b.Item2 == currentDepth).ToList())
            {
                switch (input[beam.Item2][beam.Item1])
                {
                    case '.':
                    case 'S':
                        currentBeams.Add((beam.Item1, beam.Item2 + 1));
                        break;
                    case '^':
                        currentBeams.Add((beam.Item1 + 1, beam.Item2 + 1));
                        currentBeams.Add((beam.Item1 - 1, beam.Item2 + 1));
                        splitAmount++;
                        Console.WriteLine("Split at: ({0}, {1})", beam.Item1, beam.Item2);
                        break;
                }
            }
        }

        Console.WriteLine(splitAmount);
    }

    static void Day7Part2Broken()
    {
        var input = ReadInput(7);
        var depth = input.Length;

        var amountOfTimelines = 0;
        ContinueDown((input[0].IndexOf('S'), 0));

        void ContinueDown((int, int) beam)
        {
            if (beam.Item2 == depth)
            {
                amountOfTimelines++;
                if (amountOfTimelines % 1000000 == 0)
                    Console.WriteLine(amountOfTimelines);
                return;
            }

            if (input[beam.Item2][beam.Item1] == '^')
            {
                ContinueDown((beam.Item1 + 1, beam.Item2));
                ContinueDown((beam.Item1 - 1, beam.Item2));
            }
            else
            {
                ContinueDown((beam.Item1, beam.Item2 + 1));
            }
        }

        Console.WriteLine(amountOfTimelines);
    }

    static void Day7Part2()
    {
        var input = ReadInput(7);
        List<Beam> currentBeams =
        [
            new()
            {
                X = input[0].IndexOf('S'),
                Y = 0,
                Thickness = 1,
            },
        ];

        var currentDepth = 0;

        for (; currentDepth < input.Length; currentDepth++)
        {
            foreach (var beam in currentBeams.ToList())
            {
                switch (input[beam.Y][beam.X])
                {
                    case '.':
                    case 'S':
                        var nextBeam = currentBeams.Find(b => b.X == beam.X && b.Y == beam.Y + 1);
                        if (nextBeam == null)
                            beam.Y++;
                        else
                        {
                            nextBeam.Thickness += beam.Thickness;
                            currentBeams.Remove(beam);
                        }

                        break;
                    case '^':
                        nextBeam = currentBeams.Find(b => b.X == beam.X + 1 && b.Y == beam.Y + 1);
                        if (nextBeam == null)
                        {
                            currentBeams.Add(
                                new Beam
                                {
                                    X = beam.X + 1,
                                    Y = beam.Y + 1,
                                    Thickness = beam.Thickness,
                                });
                            currentBeams.Remove(beam);
                        }
                        else
                        {
                            nextBeam.Thickness += beam.Thickness;
                            currentBeams.Remove(beam);
                        }

                        nextBeam = currentBeams.Find(b => b.X == beam.X - 1 && b.Y == beam.Y + 1);
                        if (nextBeam == null)
                        {
                            currentBeams.Add(
                                new Beam
                                {
                                    X = beam.X - 1,
                                    Y = beam.Y + 1,
                                    Thickness = beam.Thickness,
                                });
                            currentBeams.Remove(beam);
                        }
                        else
                        {
                            nextBeam.Thickness += beam.Thickness;
                            currentBeams.Remove(beam);
                        }

                        break;
                }
            }
        }

        Console.WriteLine(currentBeams.Sum(b => b.Thickness));
    }

    static void Day8(bool part2 = false)
    {
        var input = ReadInput(8);
        var positions = input
            .Select(line => line
                .Split(','))
            .Select(coords => new Vector3Int(
                    int.Parse(coords[0]),
                    int.Parse(coords[1]),
                    int.Parse(coords[2])
                )
            ).ToList();

        var connections = positions
            .SelectMany(p => positions.Where(q => q != p), (a, b) => (a, b))
            .Select(t => (t.Item1, t.Item2, Vector3Int.Distance(t.Item1, t.Item2)))
            .OrderBy(t => t.Item3)
            .ToList();

        // remove duplicate connections
        connections = connections.GroupBy(t => t.Item3).Select(g => g.First()).ToList();

        var circuits = positions.Select(p => new List<Vector3Int> { p }).ToList();

        var limit = part2 ? int.MaxValue : input.Length == 20 ? 10 : 1000;

        for (var index = 0; index < limit; index++)
        {
            var (a, b, distance) = connections[index];
            // Skip if both a and b already belong to the same circuit
            if (circuits.Any(c => c.Contains(a) && c.Contains(b))) continue;

            // Find the circuits containing a and b
            var circuitWithA = circuits.Find(c => c.Contains(a));
            var circuitWithB = circuits.Find(c => c.Contains(b));

            if (part2 && circuits.Count == 2)
            {
                // ReSharper disable once RedundantCast
                Console.WriteLine("Part 2: {0}", (long)a.X * (long)b.X);
                // 8199963486
                return;
            }

            // Ensure both circuits exist before proceeding
            if (circuitWithA != null && circuitWithB != null && circuitWithA != circuitWithB)
            {
                Console.WriteLine("Connecting {0} and {1}", a, b);
                circuits.Remove(circuitWithB);
                circuitWithA.AddRange(circuitWithB);
            }
            else
            {
                Console.WriteLine("Error: Circuit not found for {0} and {1}", a, b);
                break;
            }
        }

        Console.WriteLine(circuits.Select(c => c.Count).Order().Reverse().Take(3).Aggregate((a, b) => a * b));
        // 26400
    }
}

class Beam
{
    public int X { get; set; }
    public int Y { get; set; }

    public long Thickness { get; set; }
}

class Vector3Int(int x, int y, int z)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public int Z { get; set; } = z;

    private Vector3Int(Vector3Int v) : this(v.X, v.Y, v.Z)
    {
    }

    public Vector3Int Clone()
    {
        return new Vector3Int(this);
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }

    public static Vector3Int operator +(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vector3Int operator -(Vector3Int a, Vector3Int b)
    {
        return new Vector3Int(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Vector3Int operator *(Vector3Int a, int scalar)
    {
        return new Vector3Int(a.X * scalar, a.Y * scalar, a.Z * scalar);
    }

    public static Vector3Int operator /(Vector3Int a, int scalar)
    {
        return new Vector3Int(a.X / scalar, a.Y / scalar, a.Z / scalar);
    }

    public static double Distance(Vector3Int a, Vector3Int b)
    {
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
    }
}