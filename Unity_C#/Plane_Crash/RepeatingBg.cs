using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Repositioning background along X axis while moving
 */

public class RepeatingBg : MonoBehaviour {

    private BoxCollider2D groundCollider;
    private float groundHorizontalLen;

	// Use this for initialization
	void Start ()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        groundHorizontalLen = groundCollider.size.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (transform.position.x < -groundHorizontalLen)
        {
            RepositionBg();
        }
	}
    private void RepositionBg()
    {
        Vector2 groundOffset = new Vector2(groundHorizontalLen * 2f, 0);
        transform.position = (Vector2)transform.position + groundOffset;
    }
}
