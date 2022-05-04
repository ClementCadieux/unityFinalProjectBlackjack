using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    Canvas pauseMenu;

    [SerializeField]
    Canvas gameCanvas;

    [SerializeField]
    GameObject gameObject1;

    [SerializeField]
    GameObject gameObject2;

    [SerializeField]
    GameObject gameObject3;

    public bool GameIsPaused = false;

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0;
        pauseMenu.enabled = true;
        gameCanvas.enabled = false;
        gameObject1.SetActive(false);
        gameObject2.SetActive(false);
        gameObject3.SetActive(false);
        
    }

    public void Resume()
    {
        GameIsPaused = false;
        Time.timeScale = 1;
        pauseMenu.enabled = false;
        gameCanvas.enabled=true;
        gameObject1.SetActive(true);
        gameObject2.SetActive(true);
        gameObject3.SetActive(true);
    }
}
