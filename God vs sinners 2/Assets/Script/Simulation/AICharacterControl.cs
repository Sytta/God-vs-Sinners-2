using System;
using System.Collections.Generic;
/// <summary>
/// Genesis controller a pedestrian
/// TODO Convert with pathdfinder.directionPed and maneuvers
/// </summary>
public class AICharacterControl : SimulationObject
{
    public double dt = 0.05;
    public double timeElapsed = 0;
    //double dt=0.03; // second
    public double Ti = 0.5; // speed 
    public double Ai = 0.05;  // Newton
    public double Bi = 0.5; // metres
    public double minDistanceInteraction = 3;
    public double minDistanceInteractionSqrt = 9;

    public double minDistanceInteractionWall = 2;

    public double K = 1.2 * 1000; // to be estimated (kg/s2)
    public double k2 = 1; // to be estimated (kg/m.s) panic situation
    public double kwall = 2.4 * 10000;
    public double k2wall = 1;
    public Vector3G F1;
    private bool destroy;
    public Vector3G F2, F3;
    public int nbObjects;
    public Vector3G V;
    public Vector3G foward;
    public Vector3G speed;
    public double distance = 1;

    public double panic = 0;
    public double minPanic = 0, maxPanic = 10;

    public double minSpeed = 1.5, maxSpeed = 6;

    public double accelToCenter = 0.5;


    public double defaultDist = 3;

    public Vector3G destination = null;
    public enum State { WAITINGONNODE, ONLINK };
    public State state;

    public double accelMax = 2;

    private static System.Random rnd = new System.Random();
    public static float genRandomFloat(float min, float max)
    {
        // Perform arithmetic in double type to avoid overflowing
        double range = (double)max - (double)min;
        double sample = rnd.NextDouble();
        double scaled = (sample * range) + min;
        float f = (float)scaled;

        return f;
    }


    /// <summary>
    /// Basic Constructor 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="foward"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public AICharacterControl(Vector3G position, Vector3G foward, long idS)
    {
        this.position = position;
        this.foward = foward;
        this.speed = new Vector3G(0, 0, 0);

        F1 = new Vector3G(0, 0, 0);
        F2 = new Vector3G(0, 0, 0);
        F3 = new Vector3G(0, 0, 0);

        V = new Vector3G(0, 0, 0);

        // get the components on the object we need ( should not be null due to require component so no need to check )
        id = idS;
        minDistanceInteractionSqrt = minDistanceInteraction * minDistanceInteraction;
        state = State.WAITINGONNODE;

        destination = new Vector3G(genRandomFloat(-45, 45), 0, genRandomFloat(-45, 45));
    }


    public void goTo(Vector3G dest, double accelSpeed)
    {
        position = dest;
        accelToCenter = accelSpeed;
    }

    public double getMaxSpeed()
    {
        return ((panic - minPanic) / (maxPanic - minPanic)) * (maxSpeed - minSpeed) + minSpeed;
    }

    /// <summary>
    /// Main update function called by genesis
    /// </summary>
    /// <param name="deltaT"></param>
    /// <returns></returns>
    public override bool genesisUpdate(double deltaT)
    {

        if (destination != null)
        {
            if ((destination - position).Magnitude() < 2)
            {
                destination = new Vector3G(genRandomFloat(-45, 45), 0, genRandomFloat(-45, 45));
            }
            F1 = destination - position;
            F1.Normalize();
            F1 *= accelToCenter;
        }
        V = (F1 + F2 + F3);
        F1.Set(0, 0, 0);
        F2.Set(0, 0, 0);
        F3.Set(0, 0, 0);

        return true;

    }

    /// <summary>
    /// Update function to feed back the current position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="foward"></param>
    /// <param name="speed"></param>
    public void update(Vector3G position, Vector3G foward, Vector3G speed)
    {
        this.position = position;
        this.foward = foward;
        this.speed = speed;
    }


    /// <summary>
    /// Interact with another character
    /// </summary>
    /// <param name="s"></param>
    /// <param name="deltaT"></param>
    public override void interactPedestrian(SimulationObject s, double deltaT)
    {


        Vector3G deltaVec = position - s.position;
        double distancePed1Ped2Sqrt = deltaVec.sqrMagnitude();
        if (this != s &&
            distancePed1Ped2Sqrt < minDistanceInteractionSqrt)
        {
            double distancePed1Ped2 = deltaVec.Magnitude();
            double repulsiveFroce = Ai * Math.Exp((distancePed1Ped2));
            Vector3G n = deltaVec / distancePed1Ped2;

            F2 += (double)(repulsiveFroce) * n;
            //            if(s.GetType() == this.GetType())
            //                F3 += avoid((AICharacterControl)s);

        }
    }

    /// <summary>
    /// Avoid another character
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private Vector3G avoid(AICharacterControl s)
    {
        Vector3G deltaVec = position - s.position;
        if (Vector3G.Dot(deltaVec, foward) > 0)
        {
            Vector3G deltaSpeed = s.speed - speed;
            Vector3G projFowardSpeed = Vector3G.Dot(foward, deltaSpeed) * foward;
            if (Vector3G.Dot(foward, projFowardSpeed) < 0)
            {
                Vector3G right = Vector3G.Cross(foward, new Vector3G(0, 1, 0)).Normalized();
                Vector3G projFowardDist = Vector3G.Dot(foward, deltaVec) * foward;
                Vector3G projRightDist = Vector3G.Dot(right, deltaVec) * right;
                if (projRightDist.Magnitude() < 1.0)
                    return right * (3.0f - projRightDist.Magnitude()) * projFowardSpeed.Magnitude() / projFowardDist.Magnitude();
            }
        }
        return new Vector3G(0, 0, 0);
    }


    /// <summary>
    /// Returns the type of the object
    /// </summary>
    /// <returns></returns>
    public override OBJECTTYPE getType()
    {
        return OBJECTTYPE.PED;
    }


}
