using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initializeAgents : MonoBehaviour {
    public Transform Agent1;

    // Use this for initialization
    void Start () {

        for (int i = 0; i<5; i++)
        {
            Instantiate(Agent1, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update () {
        SimulationMap.Instance.Update(UnityEngine.Time.deltaTime);
	}
}
