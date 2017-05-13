using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : CREATE A BOARD OBJECT FOR THE RANDOM GENERATION OF AGENTS
public static class GlobalVariablesTemp
{
    public static float mineBoardX = 0;
    public static float maxBoardX = 1;

    public static float minBoardY = 0;
    public static float maxBoardY = 1;


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
        pos.x = (float)agentID;
        pos.y = (float)agentID;
        pos.z = 0.0f;

        bodyForVec.x = 1.0f;
        bodyForVec.y = 0.0f;
        bodyForVec.z = 0.0f;

        velocity.x = 1.0f;
        velocity.y = 1.0f;
        velocity.z = 1.0f;

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

        gameObject.transform.position = self.pos;

        Vector3G posVector3G = Utilities.convert(self.pos);
        Vector3G forVecVector3G = Utilities.convert(self.bodyForVec);
        Vector3G velocityVector3G = Utilities.convert(self.velocity);

        selfSimObject.update(posVector3G, forVecVector3G, velocityVector3G);

        self.velocity += Utilities.convert(selfSimObject.V)* UnityEngine.Time.deltaTime;
    }
}
