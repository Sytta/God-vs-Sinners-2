using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class Touched : MonoBehaviour {
    string controllerHand;
	// Use this for initialization
	void Start () {
        if (this.ToString().Contains("left"))
            controllerHand = "Left";
        else
            controllerHand = "Right";
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.LogError("Need to have VRTK_ControllerEvents component attached to the controller");
            return;
        }

        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(GodKill);
        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(PickUp);
        GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(Release);
        Debug.Log("I am alive !");
    }

    private void GodKill(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        GameObject inch = GetComponent<VRTK_InteractTouch>().GetTouchedObject();
        if(inch != null)
        { 
            if (inch.tag == "inch")
                GameObject.Destroy(inch.gameObject);
        }
    }
    private void PickUp(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        GameObject inch = GetComponent<VRTK_InteractTouch>().GetTouchedObject();
        if (inch != null)
        {
            if (inch.tag == "inch")
            {
                inch.GetComponent<agentBehaviorTest>().enabled = false;
                inch.gameObject.transform.parent = this.transform;
            }
        }
    }
    private void Release(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform inch = this.transform.GetChild(i);
            if (inch != null)
            {
                if (inch.gameObject.tag == "inch")
                {
                    float y = inch.position.y;
                    inch.parent = GameObject.FindGameObjectWithTag("Table").transform;
                    inch.gameObject.GetComponent<agentBehaviorTest>().enabled = true;
                    inch.position = new Vector3(inch.position.x, y, inch.position.z);
                }
            }
        }
    }
    // Update is called once per frame
    void Update ()
    {

    }
}
