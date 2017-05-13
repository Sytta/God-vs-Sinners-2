using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class Touched : MonoBehaviour {


	// Use this for initialization
	void Start () {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.LogError("Need to have VRTK_ControllerEvents component attached to the controller");
            return;
        }

        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(GodKill);
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

    // Update is called once per frame
    void Update ()
    {

    }
}
