using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseImg : MonoBehaviour {

    public GameObject pause_on, pause_off;
    private bool pause = false;
    public GameObject pauseMenu;

    private void Update()
    {
        pause = pauseMenu.activeSelf;
        if (!pause)
        {
            pause_off.SetActive(true);
            pause_on.SetActive(false);
        }
        else
        {
            pause_off.SetActive(false);
            pause_on.SetActive(true);
        }
    }

}
