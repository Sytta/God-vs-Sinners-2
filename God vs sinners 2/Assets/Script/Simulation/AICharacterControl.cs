using System;
using System.Collections.Generic;
/// <summary>
/// Genesis controller a pedestrian
/// TODO Convert with pathdfinder.directionPed and maneuvers
/// </summary>
using UnityEngine;
public class AICharacterControl : SimulationObject
{

    public double dt = 0.05;
    public double timeElapsed = 0;
    //double dt=0.03; // second
    public double Ti = 0.5; // speed 
    public double Ai = 5;  // Newton
    public double Bi = 0.5; // metres
    public double minDistanceInteraction = 4;
    public double minDistanceInteractionSqrt = 4*4;

    public double minDistanceInteractionWall = 2;

    public Dictionary<string, Vector3G> forces;

    public double K = 1.2 * 1000; // to be estimated (kg/s2)
    public double k2 = 1; // to be estimated (kg/m.s) panic situation
    public double kwall = 2.4 * 10000;
    public double k2wall = 1;
    public Vector3G F1;
    private bool destroy;
    public Vector3G F2, F3,F4, F5attraction;
    public int nbObjects;
    public Vector3G V;
    public Vector3G foward;
    public Vector3G speed;
    public double distance = 1;

    public Vector3G raycastHit;

    public double avoidWall = 1;

    public double panic = 0.1;
    public double minPanic = 0.1, maxPanic = 10;

    public double minSpeed = 1.5, maxSpeed = 6;

    public double accelToCenter = 0.5;

    public double defaultDist = 3;

    public Vector3G destination = null;
    public enum State { WAITINGONNODE, ONLINK };
    public State state;

    public double accelMax = 2;
    public double panicDecay = 0.5;

    private static System.Random rnd = new System.Random();

    public double matingCooldown = 0;


    public bool dead=false;
    public double age;
    public DNA dna;
    public bool hasMate = false;
    public bool isMating = false;
    public AICharacterControl mate = null;
    // Reproduction
    private DNA mateDNA;
    public bool hasMated = false;


    public double morality;

    public double specialActionCooldown=30;
    public bool specialActionStarted = false;

    public AICharacterControl specialActionTarget=null;

    public bool isBeingLocked = false;

    public double specialActionDuration=2;

    public float scale;
    public static float genRandomFloat(float min, float max)
    {
        // Perform arithmetic in double type to avoid overflowing
        double range = (double)max - (double)min;
        double sample = rnd.NextDouble();
        double scaled = (sample * range) + min;
        float f = (float)scaled;

        return f;
    }
    
    public enum deathCauses
    {
        AGE,INFIDEL,GOD,OTHER
    }

    public deathCauses deathCause;


    public string getDeathCauseString()
    {
        if (!isDead())
            return null;
        else
        {
            switch (deathCause)
            {
                case deathCauses.AGE:
                    return "Old age";
                case deathCauses.GOD:
                    return "Smited by god";
                case deathCauses.INFIDEL:
                    return "Killed by filthy infidels";
                default:
                    return "Killed by reasons unknowns";

            }

        }
    }
    Vector3 F1O, F2O, F3O, F4O, F5O;
    /// <summary>
    /// Basic Constructor 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="foward"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public AICharacterControl(Vector3G position, Vector3G foward, long idS, DNA dna,float scale)
    {
        this.forces = new Dictionary<string, Vector3G>();

        morality = dna.GetMorality();
        specialActionCooldown = genRandomFloat(15, 30);
        age = genRandomFloat(0,dna.GetMortality());
        this.position = position;
        this.foward = foward;
        this.speed = new Vector3G(0, 0, 0);

        F1 = new Vector3G(0, 0, 0);
        F2 = new Vector3G(0, 0, 0);
        F3 = new Vector3G(0, 0, 0);
        F4 = new Vector3G(0, 0, 0);
        F5attraction = new Vector3G(0, 0, 0);



        V = new Vector3G(0, 0, 0);
        raycastHit  = new Vector3G(1000, 1000, 1000);

        this.dna = dna;
        this.mateDNA = null;
        this.scale = scale;
        Debug.Log(scale);
    // get the components on the object we need ( should not be null due to require component so no need to check )
        id = idS;   
        minDistanceInteractionSqrt = minDistanceInteraction * minDistanceInteraction;
        state = State.WAITINGONNODE;
        destination = new Vector3G(genRandomFloat((-1.0f / scale) * 0.8f, (1.0f / scale) * 0.8f), 0, genRandomFloat((-1.0f / scale) * 0.8f, (1.0f / scale) * 0.8f));
    }


    public void goTo(Vector3G dest, double accelSpeed)
    {
        position = dest;
        accelToCenter = accelSpeed;
    }

    public void fleeFrom(Vector3G source, double magnitudeRadius)
    {
        if (forces.ContainsKey("flee"))
        {
            forces.Remove("flee");
        }

        Vector3G deltaVec = position-source;
        double dist = (deltaVec).Magnitude();
        panic = (1 - dist / magnitudeRadius) * 10;

        panic = Utilities.Clamp<double>(panic, minPanic, maxPanic);
        if (panic > 1)
        {
            forces.Add("flee", deltaVec);
        }

    }
    public bool isDead()
    {
        return dead;

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
        if ((morality < 25 || morality > 75) && !specialActionStarted)
            specialActionCooldown -= deltaT;


        if (specialActionStarted)
        {
            specialActionDuration -= deltaT;
            if(specialActionDuration < 0)
            {
                if (morality < 25)
                {
                    specialActionTarget.dead = true;
                    specialActionTarget.deathCause = deathCauses.INFIDEL;
                    morality -= 5;
                    morality = Math.Max(0, morality);
                    SimulationMap.Instance.fleeFrom(specialActionTarget.position, 8);
                }
                else
                {
                    morality += 5;
                    morality = Math.Min(100, morality);
                    specialActionTarget.morality += 5;
                    specialActionTarget.morality = Math.Min(100, morality);


                }

                specialActionTarget.isBeingLocked = false;
                specialActionCooldown = 30;
                specialActionStarted = false;
                specialActionTarget = null;
                isBeingLocked = false;
                specialActionDuration = 2;
            }
        }

       

        age += deltaT;
        if (age > dna.GetMortality())
        {
            dead = true;
            deathCause = deathCauses.AGE;

        }
            
        matingCooldown -= deltaT;
        Vector3G deltaVec = position - raycastHit;
        double toWall = deltaVec.Magnitude();
        if (toWall < 2)
        {
            F2 = deltaVec.Normalized() / toWall* avoidWall;
        }

        if (destination != null)
        {
            if ((destination - position).Magnitude() < 2)
            {
                destination = new Vector3G(genRandomFloat( (-1.0f / scale) * 0.8f, (1.0f / scale) * 0.8f), 0, genRandomFloat((-1.0f / scale) * 0.8f, (1.0f / scale) * 0.8f));
            }
            F1 = destination - position;
            F1.Normalize();
            F1 *= accelToCenter;
        }

        panic -= panicDecay * deltaT;
        panic += deltaPanic;
        deltaPanic = 0;

        panic = Utilities.Clamp<double>(panic, minPanic, maxPanic);

        
        V = (F1 + F2 + F3 + F4 + F5attraction);
        foreach (KeyValuePair<string, Vector3G> entry in forces)
        {
            if (entry.Key.Equals("flee"))
            {
                V += entry.Value * panic * 0.5;
            }
            else
            {
                V += entry.Value;
            }
        }
        if(panic < 0.5 && forces.ContainsKey("flee"))
            forces.Remove("flee");

        F1O = Utilities.convert(F1);
        F2O = Utilities.convert(F2);
        F3O = Utilities.convert(F3);
        F4O = Utilities.convert(F4);
        F5O = Utilities.convert(F5attraction);

        F1.Set(0, 0, 0);
        F2.Set(0, 0, 0);
        F3.Set(0, 0, 0);
        F4.Set(0, 0, 0);
        F5attraction.Set(0, 0, 0);
        return true;

    }

    /// <summary>
    /// Update function to feed back the current position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="foward"></param>
    /// <param name="speed"></param>
    public DNA update(Vector3G position, Vector3G foward, Vector3G speed, Vector3G raycastHit)
    {
        this.position = position;
        this.foward = foward;
        this.speed = speed;
        this.raycastHit = raycastHit;
        if (hasMated)
        {
            hasMate = false;
            mate = null;
            // Reproduction
            mateDNA = null;
            hasMated = false;
            matingCooldown = 15;
        }

        // If they found their mated and has not mated, return their mate's dna
        if (isMating && hasMate)
        {
            isMating = false;
            mate.isMating = false;

            hasMated = true;
            mate.hasMated = true;
            return mateDNA;
        }
        else
        {
            return null;
        }
        
    }


    public double panicTransmission = 5;
    public double deltaPanic = 0;
    /// <summary>
    /// Interact with another character
    /// </summary>
    /// <param name="s"></param>
    /// <param name="deltaT"></param>
    public override void interactPedestrian(SimulationObject s, double deltaT)
    {


        //Transmit panic
        AICharacterControl agent = (AICharacterControl )s;

        Vector3G deltaVec = position-s.position;

        double distancePed1Ped2Sqrt = deltaVec.sqrMagnitude();
        if (this != s &&
            distancePed1Ped2Sqrt < minDistanceInteractionSqrt)
        {
            if (agent.panic > panic)
                deltaPanic = (agent.panic - panic) * panicTransmission * deltaT;
            if(agent.panic > 4 && agent.panic > panic + 1 && !forces.ContainsKey("flee") &&agent.forces.ContainsKey("flee"))
            {
                forces.Add("flee", agent.forces["flee"]);
            }

            double distancePed1Ped2 = deltaVec.Magnitude();
            double repulsiveFroce = Ai / (distancePed1Ped2* distancePed1Ped2);
            Vector3G n = deltaVec / distancePed1Ped2;

            F2 += (double)(repulsiveFroce) * n;
            //            if(s.GetType() == this.GetType())
            //                F3 += avoid((AICharacterControl)s);

            // If they are near enough and they are not mate, they mate
            if (panic < 3 && agent.panic < 3 &&!isBeingLocked && !agent.isBeingLocked && !specialActionStarted && !agent.specialActionStarted&& age>20 &&age < 200 && matingCooldown <= 0 && agent.matingCooldown <= 0 && (agent.dna.IsMale() != this.dna.IsMale()) && !agent.hasMate && !this.hasMate)
            {
                // TODO Refusal
                agent.mateDNA = this.dna;
                this.mateDNA = agent.dna;
                this.hasMate = true;
                agent.hasMate = true;
                mate = agent;
                agent.mate = this;
            }
            else if (agent == mate)
            {
                F5attraction = (double)(repulsiveFroce) * n * -2;
                if (distancePed1Ped2 < 1)
                {
                    isMating = true;
                    agent.isMating = true;
                }
            }

            if(!agent.isBeingLocked && specialActionCooldown <= 0 && !specialActionStarted && dna.GetMortality()-age > 5)
            {
                if (morality < 25)
                {
                    float epsilon = genRandomFloat(0, 1);
                    if (epsilon < (1 - (morality / 25.0f)  ))
                    {
                        Debug.Log(id + " Plotting to kill " + agent.id + "Epsilon :"+ epsilon + "Threshold: " + (1 - (morality / 25)));
                        specialActionStarted = true;
                        agent.isBeingLocked = true;
                        specialActionTarget = agent;
                    }
                }
                else
                {
                    if(panic < 3 && agent.panic < 3 && agent.morality+5< morality)
                    {
                        Debug.Log(id + " Planning to teach " + agent.id);

                        specialActionStarted = true;
                        agent.isBeingLocked = true;
                        specialActionTarget = agent;
                    }
                    
                }
                
            }
            if (specialActionStarted && agent == specialActionTarget)
            {
                F5attraction = (double)(repulsiveFroce) * n * -2;
            }
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

    public void end()
    {
        if (specialActionTarget != null)
            specialActionTarget.isBeingLocked = false;
    }

}
