using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    GameObject pauseMenuCanvas;

    public bool GameIsPaused = false;

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
        else
        {
            pause();
        }
    }

    public void pause()
    {
        Time.timeScale = 0;
        pauseMenuCanvas.SetActive(true);
        GameIsPaused = true;
    }

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        GameIsPaused=false;
        Time.timeScale = 2.5f;
    }
}
