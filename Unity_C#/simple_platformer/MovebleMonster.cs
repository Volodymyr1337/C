using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovebleMonster : MonoBehaviour {

    private float speed = 2f;

    private SpriteRenderer sprite;
    private Vector3 direction;

    private bool hit = false; // if mob collide something he's going back
    
    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        direction = transform.right;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        sprite.flipX = direction.x > 0;
        hit = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Block" || collider.gameObject.name == "level2Block" || collider.gameObject.name == "Box_collide")
        {
            if (!hit)
                direction *= -1;

            hit = true;
        }
    }
}
