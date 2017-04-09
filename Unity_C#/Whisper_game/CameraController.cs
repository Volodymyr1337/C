using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private float speed = 2f;

    private Transform target;

    public AudioClip mainSoud;
    private AudioSource audiosrc;

    private void Awake()
    {
        audiosrc = GetComponent<AudioSource>();
        if (PlayerPrefs.GetString("Sound") != "off")
        {
            audiosrc.clip = mainSoud;
            audiosrc.Play();
        }
        if (!target)
            target = FindObjectOfType<Character>().transform;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 position = target.position;
        position.z = -10.0f;
        position.y = target.position.y + 2f;
        if (target.position.x > 1 && target.position.x < 112)
        {
            transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
        }
    }
}
