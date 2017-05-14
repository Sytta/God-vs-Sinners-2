using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initializeAgents : MonoBehaviour {
    public Transform Agent1;

    // Use this for initialization
    public GameObject cubeObject;

    void Start () {

        for (int i = 0; i < 250; i++)
        {
            Transform a = Instantiate(Agent1, transform.position, transform.rotation);
            a.name = a.GetComponent<agentBehaviorTest>().self.agentID+"";
            a.parent = GameObject.FindGameObjectWithTag("Table").transform;
            a.localScale = new Vector3(1f, 1f, 1f);

            a.localPosition = new Vector3(Random.Range(-35, 35), 1, Random.Range(-35, 35));

            // CHECK IF SPAWN INSIDE COLLIDER
            // Collider[] collidersList = FindObjectsOfType(typeof(Collider)) as Collider[];
            GameObject[] collidersObjectsList = GameObject.FindGameObjectsWithTag("TempColliderCube");

            //Collider[] collidersList = FindObjectsOfType(typeof(Collider)) as Collider[];
            foreach (GameObject colliderObject in collidersObjectsList)
            {
                if (colliderObject.GetComponent<Collider>().bounds.Contains(a.position))
                {
                    Destroy(a.gameObject);
                }
            }
        }

    }

    public static void CreateChild(Transform transform, agentClassGlobal ai)
    {
        Transform agent1 = GameObject.FindGameObjectWithTag("Spawner").GetComponent<initializeAgents>().Agent1;
        Transform a = Instantiate(agent1, transform.position + new Vector3(Random.value * 0.05f, 0, Random.value * 0.05f), transform.rotation);
        a.name = a.GetComponent<agentBehaviorTest>().self.agentID + "";

        a.GetComponent<agentBehaviorTest>().setDNA(ai.dna, ai.mateDNA);
        a.parent = GameObject.FindGameObjectWithTag("Table").transform;
        a.localScale = new Vector3(1f, 1f, 1f);

        //a.localPosition = new Vector3(Random.Range(-35, 35), 1, Random.Range(-35, 35));

        // Finish mating
        ai.setMating(false);

    }

    // Update is called once per frame
    void Update () {
        SimulationMap.Instance.Update(UnityEngine.Time.deltaTime);

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            SimulationMap.Instance.fleeFrom(new Vector3(0, 0, 0), 15);
        }

        if (this.gameObject.transform.position.y < 0)
            Destroy(this.gameObject);

    }
}
