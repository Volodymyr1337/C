using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public float jumpForce;
    public Joystick joystick;

    private bool isGrounded = false;
    private bool die = false;

    public int lives = 3;

    private float lastTimeDmgRecived;

    [HideInInspector]
    public SpriteRenderer usrSprite;
    private Rigidbody2D rgbd;
    private Animator animator;

    private ParticleSystem partDuration;

    public AudioClip resSound;
    public AudioClip hitSound;
    private AudioSource audioSrc;

    private UserState state
    {
        get { return (UserState)animator.GetInteger("state"); }
        set { animator.SetInteger("state", (int)value); }
    }

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rgbd = GetComponent<Rigidbody2D>();
        usrSprite = GetComponentInChildren<SpriteRenderer>();
        partDuration = GetComponentInChildren<ParticleSystem>();
    }

    void Update ()
    {
        CheckGround();
        Movements();
        if (die && lastTimeDmgRecived + 1.5f < Time.time)
        {
            lives--;
            lastTimeDmgRecived = Time.time;
            Debug.Log(lives);
        }
    }

    private void Movements()
    {
        Vector3 dir;
        dir.x = joystick.Horizontal();
        dir.y = joystick.Vertical();
        
        if (dir.x != 0 && !die)
        {
            int k = (dir.x > 0) ? 1 : -1;
            GetComponentInChildren<SpriteRenderer>().flipX = k > 0;
            Vector3 move = new Vector3(transform.position.x + (0.08f * k), transform.position.y);
            transform.position = Vector3.MoveTowards(transform.position, move, 1f);
            state = UserState.run;
        }
        else if (isGrounded && !die)
            state = UserState.idle;

        //jump
        if (dir.y > Mathf.Abs(dir.x) && isGrounded && !die)
        {
            rgbd.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        if (colliders.Length > 1)
        {
            if (colliders[1].gameObject.name == "Ground")
                isGrounded = true;
        }
        else
            isGrounded = false;
    }
    public void ReciveDamage()
    {
        die = true;
        state = UserState.dead;
        if (PlayerPrefs.GetString("Sound") != "off")
            audioSrc.PlayOneShot(hitSound, 0.3f);
        float direction = (usrSprite.flipX == true) ? -1 : 1;
        rgbd.AddForce(transform.right * direction * 3f, ForceMode2D.Impulse);
        StartCoroutine(Die());
        StartCoroutine(Particle());
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        die = false;
        transform.position = new Vector2(15f, -0.7f);   // respawn point
        state = UserState.idle;
        partDuration.Play();
        if (PlayerPrefs.GetString("Sound") != "off")
        {
            audioSrc.PlayOneShot(resSound, 0.2f);
        }
    }
    IEnumerator Particle()
    {
        yield return new WaitForSeconds(2f);
        partDuration.Stop();
    }


}
public enum UserState
    {
        idle, run, dead  
    }
