﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initializeAgents : MonoBehaviour {
    public Transform Agent1;

    // Use this for initialization
    public GameObject cubeObject;

    void Start () {

<<<<<<< HEAD
        for (int i = 0; i < 256; i++)
=======
        for (int i = 0; i<200    ; i++)
>>>>>>> origin/master
        {
            Transform a = Instantiate(Agent1, transform.position, transform.rotation);
            a.parent = GameObject.FindGameObjectWithTag("Table").transform;
            a.localScale = new Vector3(0.5f, 1.7f, 0.5f);

            a.localPosition = new Vector3(Random.Range(-50.0f, 50.0f), 0, Random.Range(-50.0f, 50.0f));

            // CHECK IF SPAWN INSIDE COLLIDER
            // Collider[] collidersList = FindObjectsOfType(typeof(Collider)) as Collider[];
            GameObject[] collidersObjectsList = GameObject.FindGameObjectsWithTag("TempColliderCube");

            //Collider[] collidersList = FindObjectsOfType(typeof(Collider)) as Collider[];
            foreach (GameObject colliderObject in collidersObjectsList)
            {
                if (colliderObject.GetComponent<Collider>().bounds.Contains(a.position))
                {
                    Destroy(a.gameObject);
                }
            }
        }

    }

    // Update is called once per frame
    void Update () {
        SimulationMap.Instance.Update(UnityEngine.Time.deltaTime);
<<<<<<< HEAD
=======
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            SimulationMap.Instance.fleeFrom(new Vector3(0, 0, 0), 15);
        }

>>>>>>> origin/master
    }
}
