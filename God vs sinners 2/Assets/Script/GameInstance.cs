﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameInstance : MonoBehaviour
{
    public static GameInstance instance = null;
    public GameObject MenuMusic;
    public GameObject MainMusic;
    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToMainMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        //MenuMusic.GetComponent<AudioSource>().Play();
        //MainMusic.GetComponent<AudioSource>().Stop();
    }
    public void ToMainGame()
    {
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
        //MenuMusic.GetComponent<AudioSource>().Stop();
        //MainMusic.GetComponent<AudioSource>().Play();
    }
    public void ToEndScreen()
    {

    }
}
