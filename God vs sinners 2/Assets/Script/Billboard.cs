using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public float length = 5;
    void start()
    {

    }
    void Update()
    {
        length -= UnityEngine.Time.deltaTime;
        transform.rotation = Camera.main.transform.rotation;
        transform.Rotate(new Vector3(90, 180, 0));
        if (length < 0)
        {
            Destroy(gameObject);
        }
    }
}


