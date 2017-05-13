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
        Vector3 v = new Vector3(1, 0, 1);
        if (collision.gameObject.tag == "Table" || collision.gameObject.tag == "Untagged")
        {
            return;
        }
        Vector3 direction;
        direction = this.transform.position + collision.transform.position;
        direction.y = 0;
        direction = direction.normalized;
        ContactPoint contact = collision.contacts[0];

        GetComponent<Rigidbody>().velocity = collision.relativeVelocity.magnitude  * direction * 2.0f;

    }
}
