using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inch : MonoBehaviour {

    // Use this for initialization
    void Start () {
        
    }

    //public static Vector3 previous = new Vector3(0, 0, 0);

    // Update is called once per frame
    void Update() {
        Animator anim = gameObject.GetComponent<Animator>();
        if (anim != null)
        {
            if (gameObject.transform.position.y > 1.01 || gameObject.transform.position.y < 0.99)
            {
                anim.enabled = false;
            }
            else
            {
                anim.enabled = true;
            }

            RaycastHit hit = new RaycastHit();

            if (!Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, out hit))
            {
                anim.enabled = false;
            }
        }
        transform.GetChild(0).transform.position = transform.position;

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
        else if (collision.gameObject.tag == "TempColliderCube")
        {
            Vector3 m = collision.gameObject.transform.position - gameObject.transform.position;

            agentBehaviorTest bt = GetComponent<agentBehaviorTest>();
            if (bt.selfSimObject != null)
                bt.selfSimObject.goTo(Utilities.convert(m * 10), 1);
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
