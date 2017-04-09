using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkelWorker : MonoBehaviour {

    [SerializeField]
    private float speed;
    private Vector3 direction;
    private SpriteRenderer sprite;
    private Animator animator;

    public GameObject[] pointlight;

    private bool attack = false;
    private WorkerState state
    {
        get { return (WorkerState)animator.GetInteger("State"); }
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
        if (!attack)
            Move();
    }
    private void Move()
    {
        if (transform.position.x < 34 || transform.position.x > 50)
        {
            direction *= -1;
            pointlight[0].SetActive(direction.x < 0);
            pointlight[1].SetActive(direction.x > 0);
        }
        sprite.flipX = direction.x > 0;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
        state = WorkerState.run;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        Character user = collider.GetComponent<Character>();
        if (user && user is Character)
        {
            attack = true;
            state = WorkerState.attack;
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
public enum WorkerState
{
    run, attack
}
