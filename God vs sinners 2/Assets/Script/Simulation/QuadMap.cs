using System.Collections.Generic;
using System.Threading;
/// <summary>
/// A recursif dynamic data structure for the simulation. Will split or merge depending on the number of objects inside.
/// </summary>
public class QuadMap
{
    static bool debug = true;

    public double sizeX, sizeY;
    public double maxInteractionSize = 3;
    public double centerX, centerY;
    public int maxChild = 20;
    public int mergethreshold = 5;
    public bool isEndNode = true;
    public List<SimulationObject> childs;
    public QuadMap[] subMap;
    public QuadMap parent;
    public long id;
    public AdjacencyMap adjacencyMap;
    public Vector3 A, B, C, D;
    public bool locked = false;
    public bool DestroyChildFlag = false;

    /// <summary>
    /// Static constructor to create a new instance of quadMap. 
    /// </summary>        
    public static QuadMap Create(double sizeX, double sizeY, double centerX, double centerY, QuadMap parent, double maxInteractionSize, AdjacencyMap map)
    {
        QuadMap yourObject = new QuadMap();
        yourObject.sizeX = sizeX;
        yourObject.sizeY = sizeY;
        yourObject.centerX = centerX;
        yourObject.centerY = centerY;
        yourObject.parent = parent;
        yourObject.childs = new List<SimulationObject>();
        yourObject.subMap = new QuadMap[4];
        yourObject.maxInteractionSize = maxInteractionSize;
        yourObject.id = IdGenerator.Instance.genID();
        yourObject.adjacencyMap = map;
        yourObject.A = new Vector3(centerX - sizeX, 0, centerY - sizeY);
        yourObject.B = new Vector3(centerX - sizeX, 0, centerY + sizeY);
        yourObject.C = new Vector3(centerX + sizeX, 0, centerY + sizeY);
        yourObject.D = new Vector3(centerX + sizeX, 0, centerY - sizeY);

        return yourObject;
    }

    /// <summary>
    /// Update function, reccursif and with multithreading implementation
    /// </summary>
    /// <param name="deltaT"></param>
    public void Update(double deltaT)
    {
        if (isEndNode)
        {
            List<SimulationObject> toRemove = new List<SimulationObject>();
            for (int i = 0; i < childs.Count; i++)
            {
                childs[i].setDebug((int)id);
                if (!childs[i].genesisUpdate(deltaT))
                {
                    toRemove.Add(childs[i]);
                }
            }
            foreach (SimulationObject s in toRemove)
            {
                SimulationMap.Instance.removeFromTable(s);
                remove(s);
            }

            //TODO remove comment
            move();
        }
        else
        {
            foreach (QuadMap q in subMap)
            {
                if (SimulationMap.Instance.isWaiting())
                {
                    SimulationMap.Instance.addTask(new QuadMapUpdateTask(q, deltaT));
                }
                else
                    q.Update(deltaT);
            }
            if (DestroyChildFlag)
            {
                merge();
                if (parent != null)
                    parent.checkMerge();
            }

        }
    }

    /// <summary>
    /// Does the agent interaction
    /// </summary>
    /// <param name="deltaT"></param>
    public void interact(double deltaT)
    {
        if (isEndNode)
        {
            for (int i = 0; i < childs.Count - 1; i++)
            {
                for (int j = i + 1; j < childs.Count; j++)
                {
                    childs[i].interact(childs[j], deltaT);
                }

            }
            List<QuadMap> voisins = adjacencyMap.getMinAdjacent(this);
            foreach (QuadMap q in voisins)
            {
                foreach (SimulationObject s in q.childs)
                {
                    foreach (SimulationObject si in childs)
                    {
                        s.interact(si, deltaT);
                    }
                }
            }
        }
        else
        {
            foreach (QuadMap q in subMap)
            {
                q.interact(deltaT);
            }
        }
    }

    /// <summary>
    /// check if the number of childs in each submap is low enough to signal a merge
    /// </summary>
    /// <returns></returns>
    bool checkMerge()
    {
        if (getNumberChild() < mergethreshold)
        {
            if (subMap[0].isEndNode && subMap[1].isEndNode && subMap[2].isEndNode && subMap[3].isEndNode)
                DestroyChildFlag = true;
            return true;
        }
        return false;
    }

    /// <summary>
    /// merge the submaps into this one 
    /// </summary>
    void merge()
    {
        if (parent == null)
            GetHashCode();
        for (int i = 0; i < 4; i++)
        {
            adjacencyMap.remove(subMap[i]);
            SimulationMap.Instance.removeEndNode(subMap[i]);
        }
        adjacencyMap.add(this);
        SimulationMap.Instance.addEndNode(this);

        isEndNode = true;
        List<SimulationObject> allChild = new List<SimulationObject>();
        getAllChilds(allChild);
        subMap[0] = null;
        subMap[1] = null;
        subMap[2] = null;
        subMap[3] = null;
        foreach (SimulationObject s in allChild)
        {
            add(s);
        }
        DestroyChildFlag = false;
    }


    /// <summary>
    /// split this map into 4 submaps
    /// </summary>
    public void split()
    {

        isEndNode = false;
        //split in 4 each with 1/4 of the area of the original quadMap
        subMap[0] = Create(sizeX / 2, sizeY / 2, centerX - sizeX / 2, centerY + sizeY / 2, this, maxInteractionSize, adjacencyMap);
        subMap[1] = Create(sizeX / 2, sizeY / 2, centerX + sizeX / 2, centerY + sizeY / 2, this, maxInteractionSize, adjacencyMap);
        subMap[2] = Create(sizeX / 2, sizeY / 2, centerX + sizeX / 2, centerY - sizeY / 2, this, maxInteractionSize, adjacencyMap);
        subMap[3] = Create(sizeX / 2, sizeY / 2, centerX - sizeX / 2, centerY - sizeY / 2, this, maxInteractionSize, adjacencyMap);


        //-----------------
        //|       |       |
        //|   0   |   1   |
        //|       |       |
        //-----------------
        //|       |       |
        //|   3   |   2   |
        //|       |       |
        //-----------------


        adjacencyMap.remove(this);
        SimulationMap.Instance.removeEndNode(this);
        for (int i = 0; i < 4; i++)
        {
            adjacencyMap.add(subMap[i]);
            SimulationMap.Instance.addEndNode(subMap[i]);
        }

        //Add all the objects into those 4 quadMaps
        foreach (SimulationObject c in childs)
        {
            for (int i = 0; i < 4; i++)
            {
                subMap[i].add(c);
            }
        }
        childs.Clear();
    }

    /// <summary>
    /// Add new object into the map
    /// </summary>
    /// <param name="s"></param>        
    public void add(SimulationObject s)
    {
        //If already too many child and still there's enough space
        if (childs.Count >= maxChild)
        {
            if (sizeX > maxInteractionSize * 6 && sizeY > maxInteractionSize * 6)
            {
                split();
            }
        }

        //Adding the object s into the quadmap
        if (isWithin(s))
        {
            //if se quadMap is a leaf node
            if (isEndNode)
            {
                foreach (SimulationObject child in childs)
                    if (child.getId() == s.getId())
                        return;
                childs.Add(s);

            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    subMap[i].add(s);
                }
            }
        }

    }

    /// <summary>
    /// Called to check if a object has left the map.
    /// </summary>
    public void move()
    {
        List<SimulationObject> toRemove = new List<SimulationObject>();

        for (int i = 0; i < childs.Count; i++)
        {
            if (!isWithin(childs[i]))
            {
                toRemove.Add(childs[i]);
            }

        }

        foreach (SimulationObject s in toRemove)
        {
            removeFromThis(s);
            move(s);
        }


    }

    /// <summary>
    /// The recursif portion of the function
    /// </summary>
    /// <param name="s"></param>
    public void move(SimulationObject s)
    {
        if (isWithin(s))
        {
            add(s);
        }
        else
        {
            if (parent != null)
            {
                parent.move(s);
            }
            else
            {
                SimulationMap.Instance.remove(s);
            }
        }
    }


    /// <summary>
    /// Return if s is within the map
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public bool isWithin(SimulationObject s)
    {
        return s.isWithin(this);
    }



    /// <summary>
    /// Gets the number of simulationObjects in this map and all submaps
    /// </summary>
    /// <returns></returns>
    public int getNumberChild()
    {
        if (isEndNode)
            return childs.Count;
        else
            return subMap[0].getNumberChild() + subMap[1].getNumberChild() + subMap[2].getNumberChild() + subMap[3].getNumberChild();
    }

    /// <summary>
    /// Remove a object recursivly from the map
    /// </summary>
    /// <param name="s"></param>
    public void remove(SimulationObject s)
    {
        if (isWithin(s))
            if (isEndNode)
            {
                childs.Remove(s);
                if (childs.Count <= mergethreshold / 2 && parent != null)
                    parent.checkMerge();
            }
            else
            {
                subMap[0].remove(s);
                subMap[1].remove(s);
                subMap[2].remove(s);
                subMap[3].remove(s);
            }
    }


    /// <summary>
    /// remove an object from this
    /// </summary>
    /// <param name="s"></param>
    public void removeFromThis(SimulationObject s)
    {
        if (isEndNode)
        {
            childs.Remove(s);
            if (childs.Count <= mergethreshold / 2 && parent != null)
                parent.checkMerge();
        }

    }

    /// <summary>
    /// get all simulationObjects in the map and all submaps
    /// </summary>
    /// <param name="allChild"></param>
    public void getAllChilds(List<SimulationObject> allChild)
    {
        if (isEndNode)
        {
            foreach (SimulationObject c in childs)
            {
                allChild.Add(c);
            }
        }
        else
        {
            foreach (QuadMap q in subMap)
            {
                q.getAllChilds(allChild);
            }
        }

    }


}
