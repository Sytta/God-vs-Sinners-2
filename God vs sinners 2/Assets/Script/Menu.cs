using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    private Color buttonDisabled;
    private Color buttonEnabled;
    public Button buttonPlay;
    public Button buttonQuit;
    public string buttonSelected = "Play";

    // Use this for initialization
    void Start()
    {
        buttonDisabled = Color.white;
        buttonEnabled = Color.green;//new Color(105, 158,45);
        
        Button play = buttonPlay.GetComponent<Button>();
        Button quit = buttonQuit.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = -Input.GetAxis("Vertical");

        ////// ----- KEYBOARD -----//////
        if (vertical < 0)
        {
            buttonPlay.GetComponent<Image>().color = buttonEnabled;
            buttonQuit.GetComponent<Image>().color = buttonDisabled;
            buttonSelected = "Play";
        }
        else if (vertical > 0)
        {
            buttonPlay.GetComponent<Image>().color = buttonDisabled;
            buttonQuit.GetComponent<Image>().color = buttonEnabled;
            buttonSelected = "Quit";
        }

        if (Input.GetButton("Jump") && buttonSelected == "Play")
        {
            GameInstance.instance.ToMainGame();
        }

        if (Input.GetButton("Jump") && buttonSelected == "Quit")
        {
            Application.Quit();
        }
        ////// ----- END KEYBOARD -----//////

        //if (raycasting to buttonPlay) {
        //    buttonPlay.GetComponent<Image>().color = buttonEnabled;
        //    buttonQuit.GetComponent<Image>().color = buttonDisabled;
        //    buttonSelected = "Play";
        //} else if (raycasting to buttonQuit) {
        //    buttonPlay.GetComponent<Image>().color = buttonDisabled;
        //    buttonQuit.GetComponent<Image>().color = buttonEnabled;
        //    buttonSelected = "Quit";
        //}

        // Start game
        //if (press && buttonSelected == "Play")
        //{
        //    GameInstance.instance.ToMainGame();
        //}

        // Quit game
        //if (press && buttonSelected == "Quit")
        //{
        //    Application.Quit();
        //}
    }
}

