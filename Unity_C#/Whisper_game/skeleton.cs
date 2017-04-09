using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeleton : MonoBehaviour {

    [SerializeField]
    private float speed;
    private Vector3 direction;
    private SpriteRenderer sprite;
    private Animator animator;

    private bool attack = false;
    private SkeletonState state
    {
        get { return (SkeletonState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        direction = transform.right;
    }
    private void Update()
    {
        if (gameObject.name =="skeleton")
        {
            if (!attack)
                Move();
        }
        else if (gameObject.name == "skeleton2")
        {
            if (!attack)
                Move2();
        }
    }
    private void Move()
    {
        if (transform.position.x < 50 || transform.position.x > 60)
        {
            direction *= -1;
        }
        sprite.flipX = direction.x > 0;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        state = SkeletonState.run;
    }
    private void Move2()
    {
        if (transform.position.x < 61 || transform.position.x > 75)
        {
            direction *= -1;
        }
        sprite.flipX = direction.x > 0;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        state = SkeletonState.run;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        Character user = collider.GetComponent<Character>();
        if (user && user is Character)
        {
            attack = true;
            state = SkeletonState.attack;
            user.ReciveDamage();
            StartCoroutine(Attack());
            sprite.flipX = !user.usrSprite.flipX; // turn sprte to the enemy
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        attack = false;
    }
}
public enum SkeletonState
{
    run, attack
}