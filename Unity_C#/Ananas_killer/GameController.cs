using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 *  Spawning prefabs in random coord
 *  writing best and currrent score
 */


public class GameController : MonoBehaviour {

    public static GameController Instance { get; set; }

    private const float SLICE_FORCE = 1000.0f;

    /* sounds */
    public AudioClip sound_miss, sound_hit;
    private float timerLast, timer = 0.2f;

    public RectTransform _image;

    private bool isPaused;
    private List<Vegetable> veggies = new List<Vegetable>();
    public GameObject vegetablePrefab;
    public Transform trail;

    private float lastSpawn;
    private float deltaSpawn = 1f;
    private Vector3 lastMousePos;
    private Collider2D[] veggiesCols;

    private int score;
    private int highscore;
    private int lives;
    public Text scoreTxt;
    public Text highscoreTxt;
    public Image[] lifepoints;
    public GameObject pauseMenu;
    public GameObject deathMenu;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        veggiesCols = new Collider2D[0];
        NewGame();
    }

    public void NewGame()
    {
        score = 0;
        lives = 3;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        scoreTxt.text = score.ToString();
        highscore = PlayerPrefs.GetInt("Score");
        highscoreTxt.text = "BEST: " + highscore.ToString();

        foreach (Image i in lifepoints)
            i.enabled = true;

        foreach (Vegetable v in veggies)
            Destroy(v.gameObject);
        veggies.Clear();
        deathMenu.SetActive(false);
    }

    private void Update()
    {
        if (isPaused)
            return;

        if (Time.time - timerLast > timer)
        {
            _image.sizeDelta = new Vector2(60, 60);
            timerLast = Time.time;
        }

        if (Time.time - lastSpawn > deltaSpawn)
        {
            Vegetable v = GetVegetable();
            float randomX = Random.Range(-1.65f, 1.65f);
            float rand = Random.Range(0f, 1f);
            v.LaunchVegetable(Random.Range(1.8f, 2.8f), randomX, -randomX, rand);
            lastSpawn = Time.time;
        }
        if (Input.GetMouseButtonDown(0))
        {
            AudioSource.PlayClipAtPoint(sound_miss, transform.position);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -1;
            trail.position = pos;
            Collider2D[] thisFramesVeggie = Physics2D.OverlapPointAll(new Vector2(pos.x, pos.y), LayerMask.GetMask("Vegetable"));
            
            if ((Input.mousePosition - lastMousePos).sqrMagnitude > SLICE_FORCE)
            {
                foreach (Collider2D c2 in thisFramesVeggie)
                {
                    for (var i = 0; i < veggiesCols.Length; i++)
                    {
                        if (c2 == veggiesCols[i])
                        {
                            int hit = c2.GetComponent<Vegetable>().Slice();
                            if (hit == 1)
                            {
                                AudioSource.PlayClipAtPoint(sound_hit, transform.position);
                                _image.sizeDelta = new Vector2(70, 70);
                                    
                            }
                        }
                    }
                }
            }
            lastMousePos = Input.mousePosition;
            veggiesCols = thisFramesVeggie;
            
        }
    }

    private Vegetable GetVegetable()
    {
        Vegetable v = veggies.Find(x => !x.isActive);

        if (v == null)
        {
            v = Instantiate(vegetablePrefab).GetComponent<Vegetable>();
            veggies.Add(v);
        }
        return v;
    }

    public void IncrementScore(int scoreAmount)
    {
        score += scoreAmount;
        scoreTxt.text = score.ToString();

        if (score > highscore)
        {
            highscore = score;
            highscoreTxt.text = "BEST: " + highscore.ToString();
            PlayerPrefs.SetInt("Score", highscore);
        }
    }

    public void LoseLifePoint()
    {
        if (lives == 0)
            return;
        lives--;
        lifepoints[lives].enabled = false;
        if (lives == 0)
            Death();
    }
    public void Death()
    {
        isPaused = true;
        deathMenu.SetActive(true);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        isPaused = pauseMenu.activeSelf;
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
    }
    public void Exit()
    {
        Application.Quit();
    }
}
