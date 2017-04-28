using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject pannel;
    public GameObject start;
    public Text ControlKeys;
    public Text SoundTxt;


    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Option()
    {
        pannel.SetActive(true);
        start.SetActive(false);
    }
    public void OptionOK()
    {
        pannel.SetActive(false);
        start.SetActive(true);
    }
    public void Keys()
    {
        if (PlayerPrefs.GetString("Keys") != "off")
        {
            PlayerPrefs.SetString("Keys", "off");
            ControlKeys.text = "Control mode: acceleration";
        }
        else
        {
            PlayerPrefs.SetString("Keys", "on");
            ControlKeys.text = "Control mode: Control Keys";
        }
        
    }
    public void Sound()
    {
        if (PlayerPrefs.GetString("Sound") != "off")
        {
            PlayerPrefs.SetString("Sound", "off");
            SoundTxt.text = "Sound: off";
        }
        else
        {
            PlayerPrefs.SetString("Sound", "yes");
            SoundTxt.text = "Sound: on";
        }
    }

}
