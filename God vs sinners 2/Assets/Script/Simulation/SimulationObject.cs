using System;

/// <summary>
/// An abstract object in the simulation
/// </summary>
public abstract class SimulationObject
{
    /// <summary>
    /// The possible types of the objects
    /// </summary>
    public enum OBJECTTYPE
    {
        NULL, PED
    };

    public int debug;

    /// <summary>
    /// For debug
    /// </summary>
    /// <param name="i"></param>
    public void setDebug(int i)
    {
        debug = i;
    }        

    protected long id=-1;
    public Vector3G position;

    /// <summary>
    /// Initiation
    /// </summary>
    public void init()
    {
    }

    /// <summary>
    /// return the id
    /// </summary>
    /// <returns></returns>
    public long getId(){
        return id;
    }

    /// <summary>
    /// return the type of the simulationObject
    /// </summary>
    /// <returns></returns>
    public virtual OBJECTTYPE getType()
    {
        return OBJECTTYPE.NULL;
    }

    /// <summary>
    /// Genesis updated
    /// </summary>
    /// <param name="deltaT"></param>
    /// <returns></returns>
    public abstract bool genesisUpdate(double deltaT);


    /// <summary>
    /// Intereaction between two object
    /// </summary>
    /// <param name="s"></param>
    /// <param name="deltaT"></param>
    public void interact(SimulationObject s, double deltaT)
    {
        switch (s.getType())
        {
            case OBJECTTYPE.NULL:
                return;

            case OBJECTTYPE.PED:
                interactPedestrian(s, deltaT);
                break;


        }
        switch (getType())
        {
            case OBJECTTYPE.NULL:
                return;

            case OBJECTTYPE.PED:
                s.interactPedestrian(this, deltaT);
                break;


        }

    }

    internal virtual void destroy()
    {

    }

    /// <summary>
    /// Interaction to a character. intended for overload
    /// </summary>
    /// <param name="s"></param>
    /// <param name="deltaT"></param>
    public virtual void interactPedestrian(SimulationObject s, double deltaT)
    {

    }
        

    /// <summary>
    /// return if the object is within the boundaries of a specific quadmap
    /// </summary>
    /// <param name="quadMap"></param>
    /// <returns></returns>
    internal virtual bool isWithin(QuadMap quadMap)
    {
        double thisX = position.x - quadMap.centerX;
        double thisY = position.y - quadMap.centerY;
        if (thisX >= -quadMap.sizeX && thisY >= -quadMap.sizeY)
        {
            if (thisX < quadMap.sizeX && thisY < quadMap.sizeY)
                return true;

        }
        return false;
    }
}