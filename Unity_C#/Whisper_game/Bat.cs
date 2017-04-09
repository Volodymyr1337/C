using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour {

    private float speed = 2f;
    private Vector3 direction;
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        direction = transform.right;
    }

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if (transform.position.x < 80 || transform.position.x > 104)
        {
            direction *= -1;
        }
        sprite.flipX = direction.x > 0;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
}
