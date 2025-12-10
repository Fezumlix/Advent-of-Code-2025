namespace Advent_of_Code_2025;

public class Vector3Int(int x, int y, int z)
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

    /***
     * Calculates the Euclidean (straight-line) distance between two points.
     */
    public static double Distance(Vector3Int a, Vector3Int b)
    {
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
    }
}

internal class Vector2Int(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    private Vector2Int(Vector2Int v) : this(v.X, v.Y)
    {
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2Int operator *(Vector2Int a, int scalar) => new(a.X * scalar, a.Y * scalar);
    public static Vector2Int operator /(Vector2Int a, int scalar) => new(a.X / scalar, a.Y / scalar);
}