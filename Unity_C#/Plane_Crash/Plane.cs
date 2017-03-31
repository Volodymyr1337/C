using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour {

    private bool isDead = false;
    private Rigidbody2D rigidbody2d;
    private Animator animationCrash;

    public float upForce;

    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animationCrash = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDead)
        {
            if (Input.GetMouseButton(0) && transform.position.y < 5)
            {
                rigidbody2d.velocity = Vector2.zero;
                rigidbody2d.AddForce(new Vector2(0, upForce));
                Quaternion rotateZ = Quaternion.AngleAxis(3, Vector3.forward);
                transform.rotation = rotateZ;
            }
            else
            {
                Quaternion rotateZ = Quaternion.AngleAxis(-3, Vector3.forward);
                transform.rotation = rotateZ;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        rigidbody2d.velocity = Vector2.zero;
        isDead = true;
        animationCrash.SetTrigger("Die");
        GameController.instance.PlaneCrashed();
    }

}
