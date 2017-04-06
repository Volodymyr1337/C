using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance2;

    [HideInInspector]
    public int sumScore;

    private void Start()
    {
        if (Instance2 != null)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance2 = this;
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        User.Instance.score = sumScore;
        foreach (var scr in User.Instance.Scoretxt)
        {
            if (scr.name == "Score")
            {
                scr.text = "Score: " + User.Instance.score.ToString();
            }
        }
    }
}
