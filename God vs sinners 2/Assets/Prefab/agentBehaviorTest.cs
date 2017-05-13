using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// TODO : CREATE A BOARD OBJECT FOR THE RANDOM GENERATION OF AGENTS
public static class globalVariablesTemp
{
    public static float minBoardX = -5.0f;
    public static float maxBoardX = 5.0f;

    public static float minBoardY = -5.0f;
    public static float maxBoardY = 5.0f;

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
}

public class agentClassGlobal
{
    public long agentID;

    // POSITIONS
    public Vector3 pos;

    // FORWARD VECTOR
    public Vector3 bodyForVec;

    public Vector3 velocity;

    public float morality;

    public agentClassGlobal()
    {
        agentID = IdGenerator.Instance.genID();

        pos.x = globalVariablesTemp.genRandomFloat(globalVariablesTemp.minBoardX*5, globalVariablesTemp.maxBoardX*5);
        pos.y = 0.5f;
        pos.z = globalVariablesTemp.genRandomFloat(globalVariablesTemp.minBoardY*5, globalVariablesTemp.maxBoardY*5);

        bodyForVec.x = 1.0f;
        bodyForVec.y = 0.0f;
        bodyForVec.z = 0.0f;

        velocity.x = 0.0f;
        velocity.y = 0.0f;
        velocity.z = 0.0f;

        morality = 0.0f;
    }

}

public class agentBehaviorTest : MonoBehaviour
{
    public agentClassGlobal self = new agentClassGlobal();

    public AICharacterControl selfSimObject;


    public Vector3 debug;
    // Use this for initialization
    void Start()
    {
        selfSimObject = Factory.generate(self);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 output = new Vector3(1000.0f, 1000.0f, 1000.0f);

        RaycastHit hitInfoCurrent;

        if (Physics.Raycast(self.pos, self.velocity.normalized, out hitInfoCurrent, 20))
        {
            output = hitInfoCurrent.point;
            if (Vector3.Distance(output, self.pos) < 0.1f) Destroy(gameObject);
        }
        else // CAN'T DETECT A COLLIDER, MUST BE OUT OF BOUNDS
        {
            Destroy(gameObject);
        }

        // calcutate new position
        if (self.velocity.magnitude > selfSimObject.getMaxSpeed())
            self.velocity=self.velocity.normalized* (float)selfSimObject.getMaxSpeed();
        self.pos = self.pos + self.velocity * UnityEngine.Time.deltaTime;


        debug = self.velocity;
        gameObject.transform.localPosition = self.pos;

        Vector3G posVector3G = Utilities.convert(self.pos);
        Vector3G forVecVector3G = Utilities.convert(self.bodyForVec);
        Vector3G velocityVector3G = Utilities.convert(self.velocity);

        selfSimObject.update(posVector3G, forVecVector3G, velocityVector3G);

        self.velocity += Utilities.convert(selfSimObject.V)* UnityEngine.Time.deltaTime;
        if (float.IsNaN(self.velocity.x))
        {
            self.velocity = new Vector3(0, 0, 0);
        }
    }
}
