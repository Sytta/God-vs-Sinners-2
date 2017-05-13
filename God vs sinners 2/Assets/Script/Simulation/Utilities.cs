using System;
using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Utilities
/// </summary>
public static class Utilities
{
    // <summary>
    /// Convert a Genesis vector3 to a unity one
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3 convert(Vector3G v)
    {
        return new Vector3((float)v.x, (float)v.y, (float)v.z);
    }


    /// <summary>
    /// Convert a Genesis vector2 to a unity vector3
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3 convert(Vector2G v)
    {
        return new Vector3((float)v.x, (float)0.0, (float)v.y);
    }

    /// <summary>
    /// Convert a Unity vector2 to a geneis one
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3G convert(Vector3 v)
    {
        return new Vector3G(v.x, v.y, v.z);
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

