using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveblemonster : MonoBehaviour {

    [SerializeField]
    private float speed;
    private Vector3 direction;
    private SpriteRenderer sprite;
    private Animator animator;

    private bool attack = false;
    private MummyState state
    {
        get { return (MummyState)animator.GetInteger("State"); }
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
        if (gameObject.name == "Mummy2")
        {
            if (!attack)
                Move2();
        }
        else
        {
            if (!attack)
                Move();
        }
    }
    private void Move()
    {
        if (transform.position.x < 19)
        {
            direction *= -1;
        }
        sprite.flipX = direction.x > 0;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        state = MummyState.run;
    }
    private void Move2()
    {
        if (transform.position.x < 80 || transform.position.x > 100)
        {
            direction *= -1;
        }
        sprite.flipX = direction.x > 0;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        state = MummyState.run;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "fonar_trigger")
        {
            direction *= -1;
        }
        Character user = collider.GetComponent<Character>();
        if (user && user is Character)
        {
            attack = true;
            state = MummyState.attack;
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
public enum MummyState
{
    run, attack
}