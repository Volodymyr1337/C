using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float speed = 15f;
    public Transform target;
    public GameObject Background;

    private float LastCamPos;
    // Update is called once per frame
    void Update()
    {
        if (LastCamPos < transform.position.y)
        {
            Background.transform.position = new Vector3(0f, Background.transform.position.y - .005f, 11f);
            LastCamPos = transform.position.y;
        }
        Vector3 position = target.position;
        position.x = 0f;
        position.z = -10.0f;
        position.y = target.position.y + 2f;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
