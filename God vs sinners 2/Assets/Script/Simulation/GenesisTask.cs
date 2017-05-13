using System.Collections.Generic;

/// <summary>
/// Abstract genesis task for multithreading
/// </summary>
public abstract class GenesisTask
{

    public abstract void execute();

}

/// <summary>
/// Quadmap updating and interaction Task
/// </summary>
public class QuadMapUpdateTask : GenesisTask
{
    QuadMap q;
    double deltaT;
    public QuadMapUpdateTask(QuadMap q, double deltaT)
    {
        this.q = q;
        this.deltaT = deltaT;

    }
    public override void execute()
    {
        if (q != null)
        {
            q.Update(deltaT);
            q.interact(deltaT);
        }
    }


}
