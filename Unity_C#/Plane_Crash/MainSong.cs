using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSong : MonoBehaviour {

    private AudioClip song;
    public GameObject crashSound;

    // Use this for initialization
    void Start () {
        song = GetComponent<AudioClip>();
	}
	
	// Update is called once per frame
	void Update () {

        if (GameController.instance.gameOver)
        {
            gameObject.SetActive(false);
            crashSound.SetActive(true);
        }
            
		
	}
}
