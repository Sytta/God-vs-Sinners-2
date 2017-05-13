using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// TODO : CREATE A BOARD OBJECT FOR THE RANDOM GENERATION OF AGENTS
public class agentClassGlobal
{
    public long agentID;

    // POSITIONS
    public Vector3 pos;

    // FORWARD VECTOR
    public Vector3 bodyForVec;

    public Vector3 velocity;

    public int morality;

    // DNA
    public DNA dna;

    public agentClassGlobal()
    {
        agentID = IdGenerator.Instance.genID();

        pos.x = 0.0f;
        pos.y = 0.0f;
        pos.z = 0.0f;

        bodyForVec.x = 1.0f;
        bodyForVec.y = 0.0f;
        bodyForVec.z = 0.0f;

        velocity.x = 0.0f;
        velocity.y = 0.0f;
        velocity.z = 0.0f;

        //dna = new DNA();

        //morality = dna.getMorality();
    }

}

public class agentBehaviorTest : MonoBehaviour
{
    public agentClassGlobal self = new agentClassGlobal();

    public AICharacterControl selfSimObject;

    public Vector3 debug;
    public Vector3 debug2;
    public Vector3 debug3;
    public float debug4;

    // Use this for initialization
    void Start()
    {
        selfSimObject = Factory.generate(self);
        self.pos = gameObject.transform.localPosition;
        self.dna = new DNA();
        self.morality = self.dna.getMorality();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 output = new Vector3(1000.0f, 1000.0f, 1000.0f);

        RaycastHit hitInfoCurrent;

        if (Physics.Raycast(gameObject.transform.localPosition, new Vector3(self.velocity.x, 0.0f, self.velocity.z).normalized, out hitInfoCurrent, Mathf.Infinity))
        {
            output = hitInfoCurrent.point;

            if (Vector3.Distance(output, gameObject.transform.localPosition) < 0.01f) Destroy(gameObject);

        }
        else if (Mathf.Abs(gameObject.transform.localPosition.x) > globalVariablesTemp.maxBoardX || Mathf.Abs(gameObject.transform.localPosition.y) > globalVariablesTemp.maxBoardY)// CAN'T DETECT A COLLIDER, MUST BE OUT OF BOUNDS
        {
            // Destroy(gameObject);
        }

        // calcutate new position
        if (self.velocity.magnitude > selfSimObject.getMaxSpeed())
            self.velocity=self.velocity.normalized* (float)selfSimObject.getMaxSpeed();
        gameObject.transform.localPosition = gameObject.transform.localPosition + self.velocity * UnityEngine.Time.deltaTime;
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, Math.Max(0.5f, gameObject.transform.localPosition.y), gameObject.transform.localPosition.z);

        debug = Utilities.convert(selfSimObject.destination);
        debug2 = self.velocity;
        debug3 = Utilities.convert(selfSimObject.V);
        debug4 = (float)selfSimObject.panic;
        gameObject.transform.localPosition = gameObject.transform.localPosition;

        Vector3G posVector3G = Utilities.convert(gameObject.transform.localPosition);
        Vector3G forVecVector3G = Utilities.convert(self.bodyForVec);
        Vector3G velocityVector3G = Utilities.convert(self.velocity);

        selfSimObject.update(posVector3G, forVecVector3G, velocityVector3G);

        self.velocity += Utilities.convert(selfSimObject.V)* UnityEngine.Time.deltaTime;
        if (float.IsNaN(self.velocity.x))
        {
            self.velocity = new Vector3(0, 0, 0);
        }
    }

    void OnDestroy()
    {
        SimulationMap.Instance.remove(self.agentID);
    }

    void forceUpdate(Vector3 newPos)
    {
        gameObject.transform.localPosition = newPos;
    }
}
