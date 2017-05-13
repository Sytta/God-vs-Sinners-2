using System;
/// <summary>
/// Genesis 3d vector
/// </summary>
public class Vector3G
{
    public double x;
    public double y;
    public double z;
    private double lastX = 0;
    private double lastY = 0;
    private double lastZ = 0;
    private double magnitude = 0;
    private double sqrtMagnitude = 0;
    private double lastSqrtMagnitude = 0;

    /// <summary>
    /// Basic constructor
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public Vector3G(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Vector3G()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    /// <summary>
    /// set the vector values
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    internal void Set(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    /// <summary>
    /// Convert a 2d vector to a 3d one
    /// Vector2.x->Vector3.x
    /// 00->Vector3.y
    /// Vector2.y->Vector3.z
    /// </summary>
    /// <param name="v2"></param>
    public Vector3G(Vector2G v2)
    {
        x = v2.x;
        y = 0;
        z = v2.y;
    }


    public static Vector3G operator +(Vector3G c1, Vector3G c2)
    {
        return new Vector3G(c1.x + c2.x, c1.y + c2.y, c1.z + c2.z);
    }

    public static Vector3G operator -(Vector3G c1, Vector3G c2)
    {
        return new Vector3G(c1.x - c2.x, c1.y - c2.y, c1.z - c2.z);
    }

    public static Vector3G operator *(Vector3G c1, double c2)
    {
        return new Vector3G(c1.x * c2, c1.y * c2, c1.z * c2);
    }

    public static Vector3G operator *(double c2, Vector3G c1)
    {
        return c1 * c2;
    }

    public static Vector3G operator /(Vector3G c1, double c2)
    {
        return new Vector3G(c1.x / c2, c1.y / c2, c1.z / c2);
    }

    public static Vector3G operator /(double c2, Vector3G c1)
    {
        return c1 / c2;
    }

    public static Vector3G operator -(Vector3G c1)
    {
        return c1 * -1;
    }

    /// <summary>
    /// Get the magnitude
    /// </summary>
    /// <returns></returns>
    public double Magnitude()
    {
        updateMag();
        return magnitude;
    }

    /// <summary>
    /// Get the magnitude
    /// </summary>
    /// <returns></returns>
    public static double Magnitude(Vector3G v)
    {
        return v.Magnitude();
    }

    /// <summary>
    /// Get the second power of the magnitude
    /// </summary>
    /// <returns></returns>
    public static double SqrtMagnitude(Vector3G v)
    {
        return v.sqrMagnitude();
    }

    /// <summary>
    /// Get the second power of the magnitude
    /// </summary>
    /// <returns></returns>
    public double sqrMagnitude()
    {
        updateSqrtMagnitude();
        return sqrtMagnitude;
    }

    /// <summary>
    /// Update the second power of the magnitude
    /// </summary>
    public void updateSqrtMagnitude()
    {
        if (!(lastX == x && lastY == y && lastZ == z))
        {
            sqrtMagnitude = x * x + y * y + z * z;
            lastX = x;
            lastY = y;
            lastZ = z;
        }
    }

    /// <summary>
    /// Update the current magnitude
    /// </summary>
    public void updateMag()
    {
        updateSqrtMagnitude();
        if (!(sqrtMagnitude==lastSqrtMagnitude))
        {
            magnitude = (double)Math.Sqrt(sqrtMagnitude);
            lastSqrtMagnitude = sqrtMagnitude;
        }
    }

    /// <summary>
    /// Get the distance between the two vector
    /// </summary>
    /// <param name="position1"></param>
    /// <param name="position2"></param>
    /// <returns></returns>
    internal static double Distance(Vector3G position1, Vector3G position2)
    {
        return Vector3G.Magnitude(position1 - position2);
    }

    /// <summary>
    /// Get the second power of the distance between the two vector
    /// </summary>
    /// <param name="position1"></param>
    /// <param name="position2"></param>
    /// <returns></returns>
    internal static double SqrtDistance(Vector3G position1, Vector3G position2)
    {
        return (position1 - position2).sqrMagnitude();
    }

    /// <summary>
    /// Normalize the vector
    /// </summary>
    /// <returns></returns>
    public Vector3G Normalized()
    {
        if (sqrMagnitude() == 1)
            return this;
        else
        {
            return this / Magnitude();
        }
    }

    /// <summary>
    /// Normalize the vector
    /// </summary>
    public void Normalize()
    {
        if (sqrMagnitude() == 1)
            return;
        else
        {
            double mag = Magnitude();
            x = x / mag;
            y = y / mag;
            z = z / mag;
        }
    }

    /// <summary>
    /// Get the distance between the current vector and the one in parameters
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public double distance(Vector3G v)
    {
        return (v - this).Magnitude();
    }

    /// <summary>
    /// Normalize the temp vector
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    internal static Vector3G Normalize(Vector3G temp)
    {
        return temp.Normalized();
    }

    /// <summary>
    /// Dot product between two vector
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    internal static double Dot(Vector3G v1, Vector3G v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    }

    /// <summary>
    /// Cross product between 2 vectors
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static Vector3G Cross(Vector3G u, Vector3G v)
    {
        return new Vector3G(u.y * v.z - u.z * v.y, -(u.x * v.z - u.z * v.x), u.x * v.y - u.y * v.x);
    }

    /// <summary>
    /// Return if line AB intersects line CD
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <param name="C"></param>
    /// <param name="D"></param>
    /// <returns></returns>
    internal static bool isIntersect(Vector3G A, Vector3G B, Vector3G C, Vector3G D)
    {
        return (ccw(A, C, D) != ccw(B, C, D)) && (ccw(A, B, C) != ccw(A, B, D));
    }

    /// <summary>
    /// Sub function on the intersect function.
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <param name="C"></param>
    /// <returns></returns>
    internal static bool ccw(Vector3G A, Vector3G B, Vector3G C)
    {
        return (C.z - A.z) * (B.x - A.x) > (B.z - A.z) * (C.x - A.x);
    }

}
