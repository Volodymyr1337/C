using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBoxSound : MonoBehaviour {
    
    public AudioClip flySound;
    private AudioSource sound;
    public bool played = false;

	void Update ()
    {
        if (transform.position.y > 0)
            played = false;

        if (transform.position.y < -0.55 && !played)
        {
            played = true;
            if (PlayerPrefs.GetString("Sound") != "off")
            {
                sound = GetComponent<AudioSource>();
                sound.PlayOneShot(flySound, .3f);
            }
        }
        
    }
}
