using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flybox : MonoBehaviour {

    public GameObject flyingBox;
    private float timeLast, deltaTime = 3f;
    Vector3 position;
    private bool fall = false;

    private void Awake()
    {
        flyingBox.SetActive(false);
        position = flyingBox.transform.position;
    }
    private void Update()
    {
        
       if (Time.time > (timeLast + deltaTime) && fall)
        {
            fall = false;
            flyingBox.SetActive(false);
            flyingBox.transform.position = position;
        } 
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "User")
        {
            fall = true;
            flyingBox.SetActive(true);
        }
        timeLast = Time.time;
    }
}
