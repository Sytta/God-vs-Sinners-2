using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyGibs : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyGibsSelf());
    }

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator DestroyGibsSelf()
    {
        yield return new WaitForSeconds(4);
        Destroy(gameObject);

    }
}
