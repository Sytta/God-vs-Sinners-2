using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
/// <summary>
/// Singleton simulation map for the whole simulation.
/// </summary>
public class SimulationMap
{

    private static SimulationMap instance;
    private int nbThreads = 7;

    public double sizeX = 1, sizeY = 1;
    public QuadMap map;
    private List<QuadMap> endNodes;
    private System.Random random;
    public double maxInteractionSize = 3;
    public int debug;
    private Dictionary<long, SimulationObject> objects;
    int count;

    private double elapsedTime;
    public int freq = 3;

    private Queue<Semaphore> QueueWaitingForWork;
    private List<Thread> ThreadsList;
    private Queue<GenesisTask> GeneralTaskQueue;

    private Semaphore waitForDoneSem;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    public SimulationMap(double posX = 0, double posY = 0, double scaleX = 10240, double scaleY = 10240)
    {
        QueueWaitingForWork = new Queue<Semaphore>();
        ThreadsList = new List<Thread>();
        GeneralTaskQueue = new Queue<GenesisTask>();

        waitForDoneSem = new Semaphore(0, 1);

        endNodes = new List<QuadMap>();
        instance = this;
        objects = new Dictionary<long, SimulationObject>();
        AdjacencyMap adj = new AdjacencyMap();
        map = QuadMap.Create(scaleX, scaleY, posX, posY, null, maxInteractionSize, adj);
        endNodes.Add(map);
        random = new System.Random();
        count = 0;
        //Create the Threads;
        for(int i = 0; i < nbThreads; i++)
        {
            GenesisThread gen = new GenesisThread();
            Thread t = new Thread(gen.Main);
            t.Start();
            ThreadsList.Add(t);
        }

    }

    internal void reset()
    {
        List<long> l= new List<long>();
        foreach (long id in objects.Keys)
            l.Add(id);
        foreach (long id in l)
        {
            objects[id].destroy();
            remove(objects[id]);
            objects.Remove(id);
        }


    }

    /// <summary>
    /// get the next task in the taskqueue
    /// </summary>
    /// <returns></returns>
    public GenesisTask getTask()
    {
            
        if (GeneralTaskQueue.Count == 0)
            return null;
        else
        {
                
            GenesisTask task;

            lock (((System.Collections.ICollection)GeneralTaskQueue).SyncRoot)
            {
                debug = 1;
                if (GeneralTaskQueue.Count != 0)
                    task = GeneralTaskQueue.Dequeue();
                else
                    task = null;
            }

            return task;
        }
    }

    /// <summary>
    /// add a task to the taskqueue, wake a thread if any is sleeping to take it.
    /// </summary>
    /// <param name="t"></param>
    public void addTask(GenesisTask t)
    {
        lock (((System.Collections.ICollection)GeneralTaskQueue).SyncRoot)
        {
            GeneralTaskQueue.Enqueue(t);
        }
        if (QueueWaitingForWork.Count!=0)
            WakeOne();
    }

    /// <summary>
    /// wake a thread
    /// </summary>
    public void WakeOne()
    {
        lock (((System.Collections.ICollection)QueueWaitingForWork).SyncRoot)
        {
            debug = 3;
            if (QueueWaitingForWork.Count != 0)
                QueueWaitingForWork.Dequeue().Release(1);
        }
    }

    /// <summary>
    /// Add a thread to the waiting queue to look for work
    /// </summary>
    public void AddToWaitingForWorkQueue()
    {
        Semaphore s = new Semaphore(0, 1);
        lock (((System.Collections.ICollection)QueueWaitingForWork).SyncRoot)
        {
            debug = 4;
            QueueWaitingForWork.Enqueue(s);
        }
        s.WaitOne();
    }

    /// <summary>
    /// return if any thread is currentlywaiting 
    /// </summary>
    /// <returns></returns>
    public bool isWaiting()
    {
        return QueueWaitingForWork.Count > 0;
    }

    /// <summary>
    /// Main update function. need to be called from something 
    /// </summary>
    /// <param name="deltaT"></param>
    public void Update(double deltaT)
    {
            
        for (int i = 0; i < nbThreads; i++)
        {
            if (!ThreadsList[i].IsAlive)
            {
                GenesisThread gen = new GenesisThread();
                Thread t = new Thread(gen.Main);
                t.Start();
                ThreadsList[i] = (t);
            }
        }
        count = (count + 1) % freq;
        elapsedTime += deltaT;
        if (count == 0)
        {
            addTask(new QuadMapUpdateTask(map,elapsedTime));
            elapsedTime = 0;
        }

    }


    /// <summary>
    /// add a object into the quadmap
    /// </summary>
    /// <param name="s"></param>
    public void add(SimulationObject s)
    {
        map.add(s);
        objects.Add(s.getId(), s);
    }

    /// <summary>
    /// remove an object from the quadmap
    /// </summary>
    /// <param name="s"></param>
    public void remove(SimulationObject s)
    {
        map.remove(s);
        objects.Remove(s.getId());

    }

    /// <summary>
    /// remove an object from the quadmap by id
    /// </summary>
    /// <param name="id"></param>
    public void remove(long id)
    {
        if (objects.ContainsKey(id))
        {
            remove(objects[id]);
            objects.Remove(id);
        }

    }

    /// <summary>
    /// remove an object but doesn't remove it from the quadmap
    /// </summary>
    /// <param name="s"></param>
    public void removeFromTable(SimulationObject s)
    {
        objects.Remove(s.getId());

    }

    /// <summary>
    /// Get an objectg by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SimulationObject get(long id)
    {
        if (objects.ContainsKey(id))
        {
            return objects[id];
        }
        else
        {
            return null;
        }
    }



    /// <summary>
    /// Retun the instance of the singleton
    /// </summary>
    public static SimulationMap Instance
    {
        get
        {
            if (instance == null)
            {
                SimulationMap s = new SimulationMap();
            }
            return instance;
        }
    }

    /// <summary>
    /// adds a quadmap into the list of endnodes
    /// </summary>
    /// <param name="q"></param>
    public void addEndNode(QuadMap q)
    {
        endNodes.Add(q);
    }

    /// <summary>
    /// remove a quadmap into the list of endnodes.
    /// </summary>
    /// <param name="q"></param>
    public void removeEndNode(QuadMap q)
    {
        endNodes.Remove(q);
    }



    public void fleeFrom(Vector3 source, double magnitudeRadius)
    {
        foreach(KeyValuePair<long,SimulationObject> entry in objects)
        {
            if(entry.Value.getType() == SimulationObject.OBJECTTYPE.PED)
            {
                ((AICharacterControl)entry.Value).fleeFrom(Utilities.convert(source), magnitudeRadius);
            }
        }
    }
}


