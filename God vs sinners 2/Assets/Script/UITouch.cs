using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class UITouch : MonoBehaviour
{
    string controllerHand;
    // Use this for initialization
    void Start()
    {
        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(GO);
        //GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(GO);
        //GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(GO);
    }

    private void GO(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        if (((GameObject)GameObject.FindGameObjectsWithTag("Menu")[0]).GetComponent<Menu>().buttonSelected == "Play")
            GameInstance.instance.ToMainGame();
        else
            Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
