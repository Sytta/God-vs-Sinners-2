using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreKeeping : MonoBehaviour {

    public int moralityMax = 0;
    public int moralityScore = 0;
    public int infidelsAlive = 0;
    public int religiousAlive = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<TextMesh>().text = "Morality Score: " + moralityScore.ToString() + "/" + moralityMax.ToString() +
                                        "\nInfidels: " + infidelsAlive.ToString() +
                                        "\nReligious: " + religiousAlive.ToString();
    }
}
