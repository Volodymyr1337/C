using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public GameObject gameOvertxt;
    public bool gameOver = false;
    public Text scoreTxt;

    private int score = 0;

    public float scrollSpeed = -1.5f;

	// Use this for initialization
	void Awake ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (gameOver && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

	}
    public void PlaneScored()
    {
        if (gameOver)
        {
            return;
        }
        score += 100;
        scoreTxt.text = "SCORE: " + score.ToString();
    }

    public void PlaneCrashed()
    {
        gameOvertxt.SetActive(true);
        gameOver = true;
    }
}
