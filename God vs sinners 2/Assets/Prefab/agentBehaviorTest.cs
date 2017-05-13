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
        self.pos = gameObject.transform.localPosition;
        self.dna = new DNA();
        self.morality = self.dna.getMorality();

        selfSimObject = Factory.generate(self,dna);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < 0)
        {
            OnDestroy();
            return;
        }
        Vector3 raycastOutput = new Vector3(1000.0f, 1000.0f, 1000.0f);

        RaycastHit hitInfoCurrent;

        if (Physics.Raycast(gameObject.transform.localPosition, new Vector3(self.velocity.x, 0.0f, self.velocity.z).normalized, out hitInfoCurrent, Mathf.Infinity))
        {
            raycastOutput = hitInfoCurrent.point;

            if (Vector3.Distance(raycastOutput, gameObject.transform.localPosition) < 0.01f) Destroy(gameObject);
        }

        // calcutate new position
        if (self.velocity.magnitude > selfSimObject.getMaxSpeed())
            self.velocity=self.velocity.normalized* (float)selfSimObject.getMaxSpeed();
        gameObject.transform.localPosition = gameObject.transform.localPosition + self.velocity * UnityEngine.Time.deltaTime;
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);

        debug = Utilities.convert(selfSimObject.destination);
        debug2 = self.velocity;
        debug3 = Utilities.convert(selfSimObject.V);
        debug4 = (float)selfSimObject.panic;
        gameObject.transform.localPosition = gameObject.transform.localPosition;

        Vector3G posVector3G = Utilities.convert(gameObject.transform.localPosition);
        Vector3G forVecVector3G = Utilities.convert(self.bodyForVec);
        Vector3G velocityVector3G = Utilities.convert(self.velocity);
        Vector3G raycastHit3G = Utilities.convert(raycastOutput);

        selfSimObject.update(posVector3G, forVecVector3G, velocityVector3G, raycastHit3G);

        self.velocity += Utilities.convert(selfSimObject.V)* UnityEngine.Time.deltaTime;
        if (float.IsNaN(self.velocity.x))
        {
            self.velocity = new Vector3(0, 0, 0);
        }
//        Vector3 v = gameObject.transform.rotation.eulerAngles;
//        if (v.x < 0) v.x += 360;
//        if (v.z < 0) v.z += 360;
//        if (v.x < 345 && v.x > 15)
//        {
//            v.x = 0;
//        }
//        if (v.z < 345 && v.z > 15)
//        {
//            v.z = 0;
//        }
//        gameObject.transform.rotation = Quaternion.Euler(v);
    }

    void OnDestroy()
    {
        SimulationMap.Instance.remove(self.agentID);
        GameObject.Destroy(gameObject);
    }
}
