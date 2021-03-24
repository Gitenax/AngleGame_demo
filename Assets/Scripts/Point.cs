using UnityEngine;

[System.Serializable]
public struct Point
{
    public Point(int newX, int newY)
    {
        X = newX;
        Y = newY;
    }

    public int X { get; set; }
    
    public int Y { get; set; }
    
    public static Point Zero => new Point(0, 0);

    public static Point Up => new Point(0, 1);

    public static Point Down => new Point(0, -1);

    public static Point Right => new Point(1, 0);
    
    public static Point TopRight => new Point(1, 1);
    
    public static Point BottomRight => new Point(1, -1);

    public static Point Left => new Point(-1, 0);
    
    public static Point TopLeft => new Point(-1, 1);
    
    public static Point BottomLeft => new Point(-1, -1);

    public static Point[] Directions => new[] {Up, Right, Down, Left};
    
    public static Point[] Diagonals => new[] {TopRight, BottomRight, BottomLeft, TopLeft};
    


    public static Point operator +(Point p1, Point p2)
    {
        var (x, y) = (p1.X + p2.X, p1.Y + p2.Y);
        return new Point(x, y);
    }
    
    public static bool operator ==(Point p1, Point p2)
    {
        return p1.X == p2.X && p1.Y == p2.Y;
    }

    public static bool operator !=(Point p1, Point p2)
    {
        return !(p1 == p2);
    }


    public static Point FromVector(Vector2 vector)
    {
        return new Point((int) vector.x, (int) vector.y);
    }

    public static Point FromVector(Vector3 vector)
    {
        return new Point((int) vector.x, (int) vector.y);
    }

    public static Point Multiply(Point point, int m)
    {
        return new Point(point.X * m, point.Y * m);
    }

    public static Point Add(Point point1, Point point2)
    {
        return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }

    public static Point Clone(Point point)
    {
        return new Point(point.X, point.Y);
    }


    public void Multiply(int m)
    {
        (X, Y) = (X * m, Y * m);
    }

    public void Add(Point point)
    {
        (X, Y) = (X + point.X, Y + point.Y);
    }

    public Vector2 ToVector()
    {
        return new Vector2(X, Y);
    }

    public override string ToString()
    {
        return $"[{X}, {Y}]";
    }

    public override bool Equals(object other)
    {
        if (other is Point point)
            return point.X == X && point.Y == Y;

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}