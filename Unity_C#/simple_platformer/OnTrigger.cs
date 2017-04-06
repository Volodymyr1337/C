using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTrigger : MonoBehaviour {

    // saving boxes position etc.
    public GameObject[] boxArray;
    private Vector3[] boxPos;
    private Quaternion[] boxRotation;
    private Vector3[] boxRgbdVel;

    private void Awake()
    {
        // writing start position of boxes
        boxPos = new Vector3[boxArray.Length];
        boxRotation = new Quaternion[boxArray.Length];
        boxRgbdVel = new Vector3[boxArray.Length];
        for (var i = 0; i < boxArray.Length; i++)
        {
            boxPos[i] = boxArray[i].transform.position;
            boxRgbdVel[i] = boxArray[i].GetComponent<Rigidbody2D>().velocity;
            boxRotation[i] = boxArray[i].transform.rotation;
        }
    }

    private void FixedUpdate()
    {
        //die on pikes
        if (User.Instance.die && (User.Instance.dieTime + 1f) < User.Instance.currTime)
        {
            float spawnX = 50f, spawnY = -3f;
            transform.position = new Vector3(spawnX, spawnY);
            User.Instance.die = false;
        }
    }

    /* --- user collide conditions --- */
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (User.Instance.reciveDmgCount < 0)
        {
            User.Instance.menu.SetActive(true);
            Time.timeScale = 0;
        }

        if (collider.gameObject.name == "Coin")
        {
            Destroy(collider.gameObject);
            User.Instance.score += 10;
            foreach (var scr in User.Instance.Scoretxt)
            {
                if (scr.name == "Score")
                {
                    if (PlayerPrefs.GetString("Sound") != "off")
                    {
                        User.Instance.sound = User.Instance.GetComponent<AudioSource>();
                        User.Instance.sound.PlayOneShot(User.Instance.coinSound, .3f);
                    }
                    scr.text = "Score: " + User.Instance.score.ToString();
                }
            }
        }
        else if (collider.gameObject.name == "Obstacle")
        {
            User.Instance.reciveDamage = true;
            User.Instance.lastTimeDmg = Time.time;
            User.Instance.ReciveDmg();
            User.Instance.rgbd.velocity = Vector3.zero;
            User.Instance.rgbd.AddForce(transform.up * 3.0f, ForceMode2D.Impulse);
        }
        else if (collider.gameObject.name == "MovebleMonster")
        {
            if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(collider.gameObject.transform.position.x)) < 0.8f)
                Destroy(collider.gameObject);
            else
            {
                User.Instance.ReciveDmg();
                User.Instance.rgbd.AddForce(transform.up * 3.0f, ForceMode2D.Impulse);
            }
        }
        else if (collider.gameObject.name == "Die1")
        {
            User.Instance.lives--;
            float spawnX = 0f, spawnY = -2f;
            transform.position = new Vector3(spawnX, spawnY);
            boxStartPos();
        }
        else if (collider.gameObject.name == "Die2")
        {
            User.Instance.lives--;
            float spawnX = 20f, spawnY = -2f;
            transform.position = new Vector3(spawnX, spawnY);
            boxStartPos();
        }
        else if (collider.gameObject.name == "Pike" && !User.Instance.die)
        {
            User.Instance.die = true;
            User.Instance.Die();
            User.Instance.ReciveDmg();
        }
        else if (collider.gameObject.name == "NextLevel")
        {
            GameManager.Instance2.sumScore = User.Instance.score;
            SceneManager.LoadScene(2);
        }
    }
    /* end of ontrigger */

    private void boxStartPos()  // box start position restoring
    {
        for (var i = 0; i < boxArray.Length; i++)
        {
            boxArray[i].transform.position = boxPos[i];
            boxArray[i].GetComponent<Rigidbody2D>().velocity = boxRgbdVel[i];
            boxArray[i].transform.rotation = boxRotation[i];
        }
    }


    
}
