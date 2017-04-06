using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class User : MonoBehaviour
{
    public static User Instance { get; set; }

    public int lives;
    [SerializeField]
    private float jumpForce, slideForce;
    [SerializeField]
    private float speed;

    [HideInInspector]
    public int score;
    
    //if last time slicing > 1sec => slicing-false and animation will be idle
    [HideInInspector]
    public float lastTimeSliding, currTime;
    private bool sliding = false;

    // recive damage constants
    [HideInInspector]
    public bool reciveDamage = false;
    [HideInInspector]
    public int reciveDmgCount = 3;
    [HideInInspector]
    public float lastTimeDmg;

    // die conditions
    [HideInInspector]
    public bool die = false;
    [HideInInspector]
    public float dieTime;

    //checking user movements constants
    [HideInInspector]
    public int checkMovements, checkLastMovements;
    private bool isGrounded = false;    // user on ground or not

    //Sounds
    public AudioClip hitSound;
    public AudioClip coinSound;
    [HideInInspector]
    public AudioSource sound;

    public GameObject menu;             // - show menu if u die
    private BoxCollider2D userCollider;
    private Vector2 oldSizeCollider;
    [HideInInspector]
    public Rigidbody2D rgbd;
    private Animator animator;
    private SpriteRenderer sprite;      // for 'flip' user sprite while moving
    [HideInInspector]
    public Text[] Scoretxt;

    private UserState state
    {
        get { return (UserState)animator.GetInteger("State"); } // return current state in animator
        set { animator.SetInteger("State", (int)value); }       // writing state in animator
    }

    private void Awake()
    {
        userCollider = GetComponent<BoxCollider2D>();
        rgbd = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        Scoretxt = FindObjectsOfType<Text>();
        Instance = this;
        oldSizeCollider = userCollider.size;
        menu.SetActive(false);
        Time.timeScale = 1;
    }
    private void FixedUpdate()
    {
        CheckGround();
        if (!isGrounded && !die)
        {
            state = UserState.jump;
            return;
        }
        // It's impossible received damage more than once for 0.5s
        if (reciveDamage && (lastTimeDmg + 0.5f) < Time.time && reciveDmgCount > 0)
        {
            reciveDmgCount--;
            reciveDamage = false;
            
        }
        
        /* --- Sliding --- */
        currTime = Time.time;
        if ((lastTimeSliding + 1f) < currTime)
        {
            sliding = false;
            if (!die)
                state = UserState.idle;
            userCollider.size = oldSizeCollider;
        }
        

        /* --- if you die young --- */
        if (lives < 0)
        {
            lives++;
            ShowLives();
            menu.SetActive(true);
            Time.timeScale = 0;
        }
        ShowLives();
    }

    public void Run(int key)
    {
        if (!reciveDamage && !sliding && !die)
        {
            checkMovements++;
            if (isGrounded)
                state = UserState.run;
            Vector3 direction = transform.right * key;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
            sprite.flipX = direction.x < 0;
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            state = UserState.jump;
            rgbd.AddForce(transform.up * jumpForce * 0.7f, ForceMode2D.Impulse);
        }
    }

    public void Slide()
    {
        if (!isGrounded)
            return;
        if (!sliding && (lastTimeSliding + 1f) < currTime)
        {
            sliding = true;
            lastTimeSliding = Time.time;

            checkMovements++;

            float slideDir;
            slideDir = (sprite.flipX) ? -1 : 1;

            //change user collider while sliding
            userCollider.size = new Vector2(userCollider.size.x, (userCollider.size.y * 0.5f));

            rgbd.AddForce(transform.right * slideForce * slideDir, ForceMode2D.Impulse);
            state = UserState.slide;
        }
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = colliders.Length > 1;
    }
    
    private void ShowLives()
    {
        foreach (var live in Scoretxt)
        {
            if (live.name == "Lives")
            {
                live.text = lives.ToString();
            }
        }
    }
    public void Die()
    {
        state = UserState.dead;
        dieTime = Time.time;
    }

    public void ReciveDmg()
    {
        if (PlayerPrefs.GetString("Sound") != "off")
        {
            sound = GetComponent<AudioSource>();
            sound.PlayOneShot(hitSound, .3f);
        }
        lives--;
        state = UserState.dead;
    }
}

public enum UserState
{
    idle, run, jump, slide, dead
}