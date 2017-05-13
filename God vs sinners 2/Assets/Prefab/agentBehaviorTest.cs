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
    public float posX;
    public float posY;
    public float posZ;

    // FORWARD VECTOR
    public float forVecX;
    public float forVecY;
    public float forVecZ;

    public float velocity;

    public float morality;

    public agentClassGlobal()
    {
        agentID = IdGenerator.Instance.genID();
        posX = (float)agentID;
        posY = (float)agentID;
        posZ = 0.0f;

        forVecX = 1.0f;
        forVecY = 0.0f;
        forVecZ = 0.0f;

        velocity = 0.0f;
        morality = 0.0f;
    }
}


public class agentBehaviorTest : MonoBehaviour
{
    public Transform Agent1;

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
}
