using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {


    public AudioSource audioSrcBuild;
    public AudioClip constraction;      // звук постройки


    static AudioPlayer instance;

    public static AudioPlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioPlayer>();
            }
            return instance;
        }
    }

    public void PlayConstractionSound()
    {
        audioSrcBuild.PlayOneShot(constraction, .3f);
    }

}
