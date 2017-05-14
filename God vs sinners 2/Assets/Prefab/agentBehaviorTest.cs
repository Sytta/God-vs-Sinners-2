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
    public DNA mateDNA;
    private bool mating;

    public float scale;

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

        mating = false;
    }

    public void setMating(bool mating)
    {
        this.mating = mating;
    }

    public bool isMating()
    {
        return this.mating;
    }

}

public class agentBehaviorTest : MonoBehaviour
{
    public agentClassGlobal self = new agentClassGlobal();

    public AICharacterControl selfSimObject;

    private const float MATING_TIME = 3;
    private float matingTimeCounter = 0;

    public Transform male_fat;
    public Transform male_normal;
    public Transform female_fat;
    public Transform female_normal;

    public Vector3 debug;
    public Vector3 debug2;
    public bool debug3;
    public AICharacterControl debug4;
    public bool debug5;
    public double debug6;


    // Use this for initialization
    void Start()
    {
        self.scale = GameObject.FindGameObjectWithTag("Table").transform.localScale.x;

        self.pos = gameObject.transform.localPosition;

        if (self.dna == null)
        {
            self.dna = new DNA();
        }
        debug6 = self.dna.GetMorality();

        self.morality = self.dna.GetMorality();

        selfSimObject = Factory.generate(self, self.dna);
        debug5 = true;

        Transform toUse = null;

        if(self.dna.IsMale())
        {
            //toUse = males[UnityEngine.Random.Range(0, males.Count - 1)];
            if (self.dna.GetWeight() > 60)
            {
                toUse = male_fat;
            } else
            {
                toUse = male_normal;
            }
        }
        else
        {
            //toUse = females[UnityEngine.Random.Range(0, females.Count - 1)];
            if (self.dna.GetWeight() > 60)
            {
                toUse = female_fat;
            }
            else
            {
                toUse = female_normal;
            }
        }


        toUse = Instantiate(toUse, transform.position, transform.rotation);
        toUse.parent = transform;
        toUse.transform.localScale= new Vector3(20, 20, 20);
        //toUse.GetComponent<Animation>().wrapMode = WrapMode.Loop;
    }

    public int idToCatch = -1;
    // Update is called once per frame
    void Update()
    {
        debug3 = self.isMating();
        if (selfSimObject.preachFlag)
        {
            selfSimObject.preachFlag = false;
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<initializeAgents>().createBubble(transform, 2);

        }

        if (selfSimObject.killFlag)
        {
            selfSimObject.killFlag = false;
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<initializeAgents>().createBubble(transform, 0);

        }

        if (self.agentID == idToCatch)
        {
            idToCatch.GetHashCode();
        }
        Vector3 norm = new Vector3((float)selfSimObject.speed.x, (float)selfSimObject.speed.y, (float)selfSimObject.speed.z);

        Vector2G zihui = new Vector2G(norm.z, norm.x);
        transform.rotation = Quaternion.Euler(0, (float)(zihui.absAngle() * 180 / System.Math.PI), 0);


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

        // When mating (during 3 seconds), the 2 avatars don't move
        if (self.isMating())
        {
            if (this.matingTimeCounter < MATING_TIME)
            {
                this.matingTimeCounter += UnityEngine.Time.deltaTime;
            }
            else
            {
                if (!self.dna.IsMale())
                {
                    initializeAgents.CreateChild(gameObject.transform, self);
                }

                //self.setMating(false);
                this.matingTimeCounter = MATING_TIME;
            }
        }
        else if (!selfSimObject.isBeingLocked)
        {
            // calcutate new position
            if (self.velocity.magnitude > selfSimObject.getMaxSpeed())
                self.velocity = self.velocity.normalized * (float)selfSimObject.getMaxSpeed();

            //if (self.velocity.y > 0) self.velocity.y = 0;


            gameObject.transform.localPosition = gameObject.transform.localPosition + self.velocity * UnityEngine.Time.deltaTime;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);

            self.velocity += Utilities.convert(selfSimObject.V) * UnityEngine.Time.deltaTime;
            if (float.IsNaN(self.velocity.x))
            {
                self.velocity = new Vector3(0, 0, 0);
            }
        }
        debug = self.velocity;
        debug2 = Utilities.convert(selfSimObject.V);


        gameObject.transform.localPosition = gameObject.transform.localPosition;

        Vector3G posVector3G = Utilities.convert(gameObject.transform.localPosition);
        Vector3G forVecVector3G = Utilities.convert(self.bodyForVec);
        Vector3G velocityVector3G = Utilities.convert(self.velocity);
        Vector3G raycastHit3G = Utilities.convert(raycastOutput);

        // Return DNA if mating
        DNA mateDNA = selfSimObject.update(posVector3G, forVecVector3G, velocityVector3G, raycastHit3G);


        if (mateDNA != null)
        {
            self.mateDNA = mateDNA;

            if (!self.isMating())
            {
                self.setMating(true);
                GameObject.FindGameObjectWithTag("Spawner").GetComponent<initializeAgents>().createBubble(transform, 1);

            }
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

        if (selfSimObject.isDead())
        {

            Destroy(gameObject);

        }

    }

    public void setDNA(DNA dna1, DNA dna2)
    {
        self.dna = DNA.Reproduce(dna1, dna2);
        // Same sex tried to reproduce, remove the agent
        if (self.dna == null)
        {
            this.OnDestroy();
        }
    }

    public GameObject gore;

    void OnDestroy()
    {
        if (selfSimObject != null)
        {
            Instantiate(gore, transform.position, transform.rotation);

            if (selfSimObject.isDead())
            {

                Debug.Log("ID:" + self.agentID + " Dead at the sweet age of:" + selfSimObject.age + ". Cause of death:  " + selfSimObject.getDeathCauseString());
            }
            else
            {
                Debug.Log("ID:" + self.agentID + " Dead at the sweet age of:" + selfSimObject.age + ". Cause of death: UNKNOWN");
            }
            selfSimObject.end();
            SimulationMap.Instance.remove(self.agentID);
            GameObject.Destroy(gameObject);
        }
    }
}