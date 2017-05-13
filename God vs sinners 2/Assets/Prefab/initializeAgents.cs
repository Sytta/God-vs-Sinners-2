using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initializeAgents : MonoBehaviour {
    public Transform Agent1;

    // Use this for initialization
    void Start () {

        for (int i = 0; i<50    ; i++)
        {
            Transform a = Instantiate(Agent1, transform.position, transform.rotation);
            a.parent = GameObject.FindGameObjectWithTag("Table").transform;
            a.localScale = new Vector3(0.5f, 1.7f, 0.5f);
        }
    }

    // Update is called once per frame
    void Update () {
        SimulationMap.Instance.Update(UnityEngine.Time.deltaTime);
	}
}
