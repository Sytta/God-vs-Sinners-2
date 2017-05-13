using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    public Transform Agent1;

    // Use this for initialization
    void Start () {
        Instantiate(Agent1, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
