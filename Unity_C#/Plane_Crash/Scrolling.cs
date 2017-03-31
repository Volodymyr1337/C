using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Scrolling screen while moving
 */

public class Scrolling : MonoBehaviour {

    private Rigidbody2D rigidbody2d;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        rigidbody2d.velocity = new Vector2(GameController.instance.scrollSpeed, 0);
    }
    private void Update()
    {
        if (GameController.instance.gameOver)
        {
            rigidbody2d.velocity = Vector2.zero;
        }
    }
}
