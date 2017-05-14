using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inch : MonoBehaviour {

    //public static Vector3 previous = new Vector3(0, 0, 0);
    private String sAnim;

    // Use this for initialization
    void Start () {
       
    }


    // Update is called once per frame
    void Update() {
        Animator anim = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        if (anim != null)
        {
            if (String.IsNullOrEmpty(sAnim))
            { 
                sAnim = anim.name;
                sAnim = sAnim.Replace("(Clone)", "");
            }
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
        setAnmationDefault();
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

    void setAnmationDefault()
    {

        Animator animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load(sAnim) as RuntimeAnimatorController;
    }
    void setAnmationAttack()
    {
        String name;
        if (sAnim.Contains("billy"))
        {
            name = "billyjean_ATTACK";
        }
        else if (sAnim.Contains("enrico"))
        {
            name = "enrico_attack";
        }
        else if (sAnim.Contains("FatJenna"))
        {
            name = "FatJenna_ATTACK";
        }
        else if (sAnim.Contains("normal_girl"))
        {
            name = "normal_girl_ATTACK";
        }
        else
        {
            name = sAnim;
        }

        Animator animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load(name) as RuntimeAnimatorController;
    }

    void setAnmationRun()
    {
        String name;
        if (sAnim.Contains("billy"))
        {
            name = "billyjean_RUN";
        }
        else if (sAnim.Contains("enrico"))
        {
            name = "enrico_RUN";
        }
        else if (sAnim.Contains("FatJenna"))
        {
            name = "FatJenna_RUN";
        }
        else if (sAnim.Contains("normal_girl"))
        {
            name = "normal_girl_RUN";
        }
        else
        {
            name = sAnim;
        }

        Animator animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load(name) as RuntimeAnimatorController;
    }

    void setAnmationPraise()
    {
        String name;
        if (sAnim.Contains("billy"))
        {
            name = "billyjean_pickmepickme";
        }
        else if (sAnim.Contains("enrico"))
        {
            name = "enrico_pickmepickme";
        }
        else if (sAnim.Contains("FatJenna"))
        {
            name = "FatJenna_pickmepickme";
        }
        else if (sAnim.Contains("normal_girl"))
        {
            name = "normal_girl_pickmepickme";
        }
        else
        {
            name = sAnim;
        }

        Animator animator = gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load(name) as RuntimeAnimatorController;
    }
}
