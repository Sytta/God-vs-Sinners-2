using System;
using System.Collections.Generic;

/// <summary>
/// Adjancy Map, for solving adjancy problems between quadmaps. One instance for for a simulationMap
/// </summary>
public class AdjacencyMap
{
    Dictionary<QuadMap, List<QuadMap>> data;

    /// <summary>
    /// Basic constructor
    /// </summary>
    public AdjacencyMap()
    {
        data = new Dictionary<QuadMap, List<QuadMap>>();
    }

    /// <summary>
    /// Adds a new QuadMap
    /// </summary>
    /// <param name="q"></param>
    public void add(QuadMap q)
    {
        List<QuadMap> lQ = new List<QuadMap>();

        foreach (KeyValuePair<QuadMap, List<QuadMap>> entry in data)
        {
            if (isAdjacent(entry.Key, q))
            {
                entry.Value.Add(q);
                lQ.Add(entry.Key);
            }
        }
        data.Add(q, lQ);

    }

    /// <summary>
    /// Removes a quadmap
    /// </summary>
    /// <param name="q"></param>
    public void remove(QuadMap q)
    {
        data.Remove(q);
        foreach (KeyValuePair<QuadMap, List<QuadMap>> entry in data)
        {
            entry.Value.Remove(q);
        }
    }
    /// <summary>
    /// Get all quadmaps adjacent to q.
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    public List<QuadMap> getAllAdjacent(QuadMap q)
    {
        List<QuadMap> ret;
        data.TryGetValue(q, out ret);
        return ret;
    }


    /// <summary>
    /// Gets the minimum quadmaps to be able to interact or be interacted by with every quadmap adjacent to itself 
    /// x = center, o = interaction, | = no interaction
    /// o o o
    /// | x o
    /// | | |
    /// </summary>
    /// <param name="q"></param>
    /// <returns></returns>
    public List<QuadMap> getMinAdjacent(QuadMap q)
    {

        List<QuadMap> values, ret;
        ret = new List<QuadMap>();
        data.TryGetValue(q, out values);
        if (values != null)
            foreach (QuadMap m in values)
            {
                if (m.centerY > q.centerY + q.sizeY || (m.centerX > q.centerX + q.sizeX && m.centerY > q.centerY - q.sizeY))
                {
                    ret.Add(m);
                }
            }
        return ret;
    }



    /// <summary>
    /// return if 2 quadmap is adjacents.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    bool isAdjacent(QuadMap a, QuadMap b)
    {
        return Math.Abs(a.centerX - b.centerX) <= a.sizeX + b.sizeX && Math.Abs(a.centerY - b.centerY) <= a.sizeY + b.sizeY;
    }
}

