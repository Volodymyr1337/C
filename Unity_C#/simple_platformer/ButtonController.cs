using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

    public void Retry()
    {
        int i = Application.loadedLevel;
        if (i == 0)
            i = 1;
        SceneManager.LoadScene(i);
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Sound()
    {
        if (PlayerPrefs.GetString("Sound") != "off")
        {
            PlayerPrefs.SetString("Sound", "off");
        }
        else
        {
            PlayerPrefs.SetString("Sound", "yes");
        }
    }
}
