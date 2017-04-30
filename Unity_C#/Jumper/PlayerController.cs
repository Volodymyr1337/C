using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; set; }
    public float jumpForce;
    private Rigidbody2D rgbd;
    private bool jump;      // проверка прыгнул или нет
    private float timer;    //таймер для прыжков
    public GameObject[] arrows;
    SpriteRenderer sprt;
    public Text scoreTxt;
    //звуки
    public AudioSource AudioSrc;
    public AudioClip audioJump;
    public AudioClip audioSpring;
    public AudioClip audioSlow;

    [HideInInspector]
    public float scores;
    private float LastScorePos, BestScore;
    // панель рестарта, если зажмурился
    public GameObject Die;
    public GameObject InputField;

    private float move;

    private float spring;   //множитель пружины

    private Animator anim;
    private UsrStates States
    {
        get { return (UsrStates)anim.GetInteger("State"); }
        set { anim.SetInteger("State", (int) value); }
    }

    // Use this for initialization
    void Start()
    {
        Die.SetActive(false);
        InputField.SetActive(false);

        anim = GetComponent<Animator>();
        Instance = this;
        spring = 1f;
        sprt = GetComponentInChildren<SpriteRenderer>();
        rgbd = GetComponent<Rigidbody2D>();

        if (PlayerPrefs.GetString("Keys") == "off")
        {
            Destroy(arrows[0]);
            Destroy(arrows[1]);
        }
        BestScore = PlayerPrefs.GetInt("Score");
    }

    void FixedUpdate()
    {
        Scoring();
        /* 
         * смена кооринат по Х, если вылетел за экран 
         */
        if (transform.position.x < -4.5f || transform.position.x > 4.5f)
            transform.position = new Vector3(transform.position.x * -1, transform.position.y);

        Jump();
        Movements();
    }
    private void Movements()
    {
        move = Input.acceleration.x;
         
        if (PlayerPrefs.GetString("Keys") == "off")
        {
            if (move > 0)
                sprt.flipX = false;
            else
                sprt.flipX = true;
            transform.Translate(move * 0.5f, 0, 0);
        }

    }
    private void Jump()
    {
        
        if (jump) 
        {
            timer += Time.deltaTime;
            if (timer >= 0.1f)  //нельзя прыгать чаще чем раз в 100 мс
            {
                if (PlayerPrefs.GetString("Sound") != "off")
                    AudioSrc.PlayOneShot(audioJump, 0.2f);

                
                rgbd.AddForce(Vector2.up * jumpForce * spring, ForceMode2D.Impulse);
                timer = 0;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        States = UsrStates.idle;
        if (collision.collider.tag == "platform" && !jump)
        {
            jump = true;
            

        }
        else if (collision.collider.tag == "SpringPlatform" && !jump)
        {
            jump = true;
            spring = 1.5f;
            if (PlayerPrefs.GetString("Sound") != "off")
                AudioSrc.PlayOneShot(audioSpring, 0.2f);
        }
        else if (collision.collider.tag == "InvisPlatform" && !jump)
        {
            Destroy(collision.gameObject);
        }
        else if (collision.collider.tag == "SlowPlatform" && !jump)
        {
            jump = true;
            spring = .7f;
            if (PlayerPrefs.GetString("Sound") != "off")
                AudioSrc.PlayOneShot(audioSlow, 1f);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        jump = false;
        States = UsrStates.jump;
        spring = 1f;    //ресетим множитель
    }
    /* считаем очки */
    private void Scoring()
    {
        if (LastScorePos < transform.position.y)
        {
            LastScorePos = transform.position.y;
            scores = LastScorePos * 100;
            scoreTxt.text = "score: " + Mathf.Round(scores).ToString();
            
        }
        //if you die young
        if ((LastScorePos - 15f) > transform.position.y)
        {
            sprt.enabled = false;
            rgbd.velocity = Vector3.zero;
            Die.SetActive(true);
            if (scores > BestScore)
            {
                InputField.SetActive(true);
                PlayerPrefs.SetInt("Score", (int)scores);
            }
        }
    }



    public void MoveRight()
    {
        move += 0.5f;
        sprt.flipX = false;
        transform.Translate(move * 0.3f, 0, 0);
    }
    public void MoveLeft()
    {
        move += -0.5f;
        sprt.flipX = true;
        transform.Translate(move * 0.3f, 0, 0);
    }
}

public enum UsrStates
{
    idle, jump
}