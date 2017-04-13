using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveblePlatform : MonoBehaviour {

    Vector3 direction;
    private float speed = 1f;
    private float timer;
    private Vector3 startPos;
    private void Start()
    {
        direction = transform.right;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        
        if ((transform.position.x > (startPos.x + 2f) || transform.position.x < (startPos.x - 2f)) && timer > 0.5f)
        {
            timer = 0;
            direction *= -1;
        }
           

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
	}
}
