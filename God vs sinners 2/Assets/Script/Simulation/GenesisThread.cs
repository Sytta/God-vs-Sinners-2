

using System.Collections.Generic;

/// <summary>
/// A genesis thread to execute the tasks inside the simulation
/// </summary>
public class GenesisThread
{
    Queue<GenesisTask> taskQueue;

    /// <summary>
    /// Constructor
    /// </summary>
    public GenesisThread()
    {
        taskQueue = new Queue<GenesisTask>();

    }

    /// <summary>
    /// main thread
    /// </summary>
    public void Main()
    {
        while (true)
        {
            GenesisTask task = null;
            if (taskQueue.Count != 0)
                task = taskQueue.Dequeue();
            else
            {
                task = SimulationMap.Instance.getTask();
            }

            if (task == null)
                SimulationMap.Instance.AddToWaitingForWorkQueue();
            else
                task.execute();
        }
    }

}

