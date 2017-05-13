using System;

/// <summary>
/// A 2D Vector for implementation
/// </summary>
public class Vector2
{
    public double x = 0;
    public double y = 0;
    private double lastX = 0;
    private double lastY = 0;

    double magnitude;
    double sqrtMagnitude;
    double lastSqrtMagnitude = 0;
    /// <summary>
    /// Normal constructor
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public Vector2(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Vector2()
    {
        x = 0;
        y = 0;
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    /// <param name="v"></param>
    public Vector2(Vector2 v)
    {
        x = v.x;
        y = v.y;
    }

    /// <summary>
    /// Convert constructor from a 3d vector
    /// Vector3.x->Vector2.x
    /// Vector3.y->discarded 
    /// Vector3.z->Vector2.y
    /// Simply discards the 3rd dimension
    /// </summary>
    /// <param name="v3"></param>
    public Vector2(Vector3 v3)
    {
        x = v3.x;
        y = v3.z;

    }

    /// <summary>
    /// set the values
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    internal void Set(double x, double y)
    {
        this.x = x;
        this.y = y;
    }


    public static Vector2 operator +(Vector2 c1, Vector2 c2)
    {
        return new Vector2(c1.x + c2.x, c1.y + c2.y);
    }

    public static Vector2 operator -(Vector2 c1, Vector2 c2)
    {
        return new Vector2(c1.x - c2.x, c1.y - c2.y);
    }

    public static Vector2 operator *(Vector2 c1, double c2)
    {
        return new Vector2(c1.x * c2, c1.y * c2);
    }

    public static Vector2 operator *(double c2, Vector2 c1)
    {
        return c1 * c2;
    }

    public static Vector2 operator /(Vector2 c1, double c2)
    {
        return new Vector2(c1.x / c2, c1.y / c2);
    }

    public static Vector2 operator /(double c2, Vector2 c1)
    {
        return c1 / c2;
    }


    public static Vector2 operator -(Vector2 c1)
    {
        return c1 * -1;
    }

    /// <summary>
    /// Get the magnitude of the vector
    /// </summary>
    /// <returns></returns>
    public double Magnitude()
    {
        updateMag();
        return magnitude;
    }

    /// <summary>
    /// Get the magnitude of the vector
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static double Magnitude(Vector2 v)
    {
        return v.Magnitude();
    }

    /// <summary>
    /// Get the sqrt of the magnitude.
    /// For performance, a*a==b.sqrtMagnitude is faster than a==b.magnitude
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static double SqrtMagnitude(Vector2 v)
    {
        return v.sqrMagnitude();
    }

    /// <summary>
    /// Get the sqrt of the magnitude.
    /// For performance, a*a==b.sqrtMagnitude is faster than a==b.magnitude
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public double sqrMagnitude()
    {
        updateSqrtMagnitude();
        return sqrtMagnitude;
    }


    /// <summary>
    /// Update the magnitude of the vector, for performance and not calculating a sqrt each time
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
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
    /// Updates the magnitude to the square
    /// </summary>
    public void updateSqrtMagnitude()
    {
        if (!(lastX == x && lastY == y))
        {
            sqrtMagnitude = x * x + y * y;
            lastX = x;
            lastY = y;
        }
    }

    /// <summary>
    /// The distance between two vector
    /// </summary>
    /// <param name="position1"></param>
    /// <param name="position2"></param>
    /// <returns></returns>
    internal static double Distance(Vector2 position1, Vector2 position2)
    {
        return Vector2.Magnitude(position1 - position2);
    }

    /// <summary>
    /// the the second power of the distance between the two vectors
    /// </summary>
    /// <param name="position1"></param>
    /// <param name="position2"></param>
    /// <returns></returns>
    internal static double SqrtDistance(Vector2 position1, Vector2 position2)
    {
        return (position1 - position2).sqrMagnitude();
    }

    /// <summary>
    /// Return the normalized vector
    /// </summary>
    /// <returns></returns>
    public Vector2 Normalized()
    {
        if (sqrMagnitude() == 1)
            return this;
        else
        {
            return this / Magnitude();
        }
    }

    /// <summary>
    /// return the angle between the two vector
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static double angle(Vector2 u, Vector2 v)
    {
        double dot = Dot(u, v);
        double mag = (u.Magnitude() * v.Magnitude());
        double acos = dot / mag;
        double ret = Math.Acos(acos);
        return ret;
    }

    /// <summary>
    /// Return the absolute angle of the vector
    /// </summary>
    /// <returns></returns>
    internal double absAngle()
    {
        if (y > 0)
            return angle(this, new Vector2(1, 0));
        else if (y < 0)
            return 2 * Math.PI - angle(this, new Vector2(1, 0));
        else if (x < 0)
            return Math.PI;
        else
            return 0;

    }

    /// <summary>
    /// rotate the vector by d angles
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    internal Vector2 rotate(double d)
    {
        double newX = x * Math.Cos(d) - y * Math.Sin(d);
        double newY = x * Math.Sin(d) + y * Math.Cos(d);
        x = newX;
        y = newY;
        return this;
    }

    /// <summary>
    /// Generate a perpendicular vector to the current one when linked to the origin
    /// </summary>
    /// <returns></returns>
    internal Vector2 genPerp()
    {
        return new Vector2(-y, x);
    }

    /// <summary>
    /// Normalize the current vector
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
        }
    }

    /// <summary>
    /// Normalize the vector
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    internal static Vector2 Normalize(Vector2 temp)
    {
        return temp.Normalized();
    }

    /// <summary>
    /// the dot product
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    internal static double Dot(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }

    /// <summary>
    /// return if the right vector is on the right of the other
    /// </summary>
    /// <param name="Other"></param>
    /// <returns></returns>
    internal bool isOnRight(Vector2 Other)
    {
        double carRot = absAngle();
        double thisAngle = Other.absAngle();
        double minLeft, minRight;

        minLeft = thisAngle - carRot;
        if (minLeft < 0)
            minLeft += 2 * Math.PI;

        minRight = carRot - thisAngle;
        if (minRight < 0)
            minRight += 2 * Math.PI;
        double steering = 0;
        if (minLeft < minRight)
            return true;
        else
            return false;

    }

    /// <summary>
    /// Get the angle between the two vector from the right
    /// </summary>
    /// <param name="Other"></param>
    /// <returns></returns>
    internal double getRightAngle(Vector2 Other)
    {
        double carRot = absAngle();
        double thisAngle = Other.absAngle();
        double minRight;
        minRight = carRot - thisAngle;
        if (minRight < 0)
            minRight += 2 * Math.PI;
        return minRight;

    }


    /// <summary>
    /// Get the angle between the two vector from the left
    /// </summary>
    /// <param name="Other"></param>
    /// <returns></returns>
    internal double getLeftAngle(Vector2 Other)
    {
        double carRot = absAngle();
        double thisAngle = Other.absAngle();
        double minLeft;
        minLeft = thisAngle - carRot;
        if (minLeft < 0)
            minLeft += 2 * Math.PI;
        return minLeft;

    }

    /// <summary>
    /// get if the two vectors are in a clockwise order
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public bool isClockwise(Vector2 v){
        if (y * v.x > x * v.y)
            return false;
        else
            return true;
    }
}
