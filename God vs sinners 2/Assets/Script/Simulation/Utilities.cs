using System;
using System.Collections.Generic;
/// <summary>
/// Utilities
/// </summary>
public static class Utilities
{

    static public bool polygonIntersect(List<Vector3> poly1, List<Vector3> poly2)
    {//TODO IMPLEMENT

        return false;
    }

    static public bool pointIntersect(Vector3 point, List<Vector3> poly2)
    {//TODO IMPLEMENT
        return false;
    }

    /// <summary>
    /// CLAMP
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="val"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0) return min;
        else if (val.CompareTo(max) > 0) return max;
        else return val;
    }

}

/// <summary>
/// A pair for easier implementation
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="U"></typeparam>
public class Pair<T, U>: IComparable<Pair<T,U>> where T : IComparable
{
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }

    /// <summary>
    /// sort by the value of FIRST
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Pair<T, U> other)
    {
        return First.CompareTo(other.First);  
    }

};

