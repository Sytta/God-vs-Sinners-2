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

        pos.x = globalVariablesTemp.genRandomFloat(globalVariablesTemp.minBoardX, globalVariablesTemp.maxBoardX);
        pos.y = 0.5f;
        pos.z = globalVariablesTemp.genRandomFloat(globalVariablesTemp.minBoardY, globalVariablesTemp.maxBoardY);

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

    // Use this for initialization
    void Start()
    {
        selfSimObject = Factory.generate(self);
    }

    // Update is called once per frame
    void Update()
    {
        // calcutate new position
        self.pos = self.pos + self.velocity * UnityEngine.Time.deltaTime;

        gameObject.transform.localPosition = self.pos;

        Vector3G posVector3G = Utilities.convert(self.pos);
        Vector3G forVecVector3G = Utilities.convert(self.bodyForVec);
        Vector3G velocityVector3G = Utilities.convert(self.velocity);

        selfSimObject.update(posVector3G, forVecVector3G, velocityVector3G);

        self.velocity += Utilities.convert(selfSimObject.V)* UnityEngine.Time.deltaTime;
    }
}
