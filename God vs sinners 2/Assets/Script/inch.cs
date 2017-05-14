using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    //public static Vector3 previous = new Vector3(0, 0, 0);

	// Update is called once per frame
	void Update () {
        //Vector3 velocity = (transform.position - previous).normalized;
        //previous = transform.position;

       // Vector3 norm = gameObject.GetComponent<Rigidbody>().velocity.normalized;

       // Vector2G zihui = new Vector2G(norm.x, norm.z);
        //transform.rotation = Quaternion.Euler(gameObject.GetComponent<Rigidbody>().velocity.x, (float)(zihui.absAngle() * (180 / System.Math.PI)), gameObject.GetComponent<Rigidbody>().velocity.z);
        //transform.rotation = Quaternion.Euler(gameObject.GetComponent<Rigidbody>().velocity.x, (float)(zihui.absAngle() * (180 / System.Math.PI)), gameObject.GetComponent<Rigidbody>().velocity.z);



    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
        {
            Debug.Log("Killed by: " + collision.gameObject.name + collision.relativeVelocity.magnitude.ToString());
            DestroyObject(gameObject);
            return;
        }
        Vector3 v = new Vector3(1, 0, 1);

        if (collision.gameObject.tag == "Table" || collision.gameObject.tag == "Untagged")
        {
            return;
        }
        else if (collision.gameObject.tag == "God")
        {


        }
        else if (collision.gameObject.tag == "projectile")
        {
            if(collision.relativeVelocity.magnitude > 0.05)
            {
                DestroyObject(gameObject);
            }

        }
        else { 
            /*
            Vector3 direction;
            direction = this.transform.position + collision.transform.position;
            direction.y = 0;
            direction = direction.normalized;
            ContactPoint contact = collision.contacts[0];

            GetComponent<Rigidbody>().velocity = collision.relativeVelocity.magnitude  * direction * 2.0f;
            */
        }
    }
}
