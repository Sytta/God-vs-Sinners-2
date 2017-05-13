using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        Vector3 direction;
        direction = this.transform.position + collision.transform.position;
        ContactPoint contact = collision.contacts[0];

        GetComponent<Rigidbody>().velocity = collision.relativeVelocity.magnitude * direction.normalized * 10f;

    }
}
