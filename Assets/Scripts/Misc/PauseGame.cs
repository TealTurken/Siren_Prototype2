using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool paused = false;

    public void Pause()
    {
        if(!paused)
        {
            Time.timeScale = 0.0f;
            paused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            paused = false;
        }
    }
}
