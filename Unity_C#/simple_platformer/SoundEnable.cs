using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEnable : MonoBehaviour {

    private Text Soundtxt;

    private void Start()
    {
        Soundtxt = GetComponent<Text>();
    }
    private void Update()
    {
        if (PlayerPrefs.GetString("Sound") == "off")
            Soundtxt.text = "Sound off";
        else
            Soundtxt.text = "Sound on";
    }

}
