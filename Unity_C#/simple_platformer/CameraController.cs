using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private Transform target;

    public AudioClip mainSound;
    private AudioSource sound;

    private void Awake()
    {
        if (!target)
            target = FindObjectOfType<User>().transform;
        if (PlayerPrefs.GetString("Sound") != "off")
        {
            sound = GetComponent<AudioSource>();
            sound.PlayOneShot(mainSound, 0.1f);
        }
    }
	// Update is called once per frame
	void Update ()
    {
        Vector3 position = target.position;
        position.z = -10.0f;
        position.y = target.position.y + 1f;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
	}
}
