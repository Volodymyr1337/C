using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawn : MonoBehaviour
{
    public GameObject Spawn;
    public GameObject Platform;
    public GameObject MoveblePlatform;
    public GameObject InvisPlatform;
    public GameObject SpringPlatform;
    public GameObject SlowPlatform;
    public Transform userpos;
    private float timer;

    public float N = 15f;

    private float Last = 0;

    void Start ()
    {
        
    }

    void Update ()
    {
        
        timer += Time.deltaTime;
        //спауним платформы перед юзером с интервалом 100мс
        if (timer > 0.1f && Mathf.Abs(userpos.position.y - transform.position.y) < 20f)
        {
            timer = 0f;
            PlatformSpawning();
        }
        
    }

    public void PlatformSpawning()
    {
        
        float current = Random.Range(-2.5f, 2.5f);
        // чтобы платформы не накладывались одна на другую сравниваем
        // если текущее положение по Х находится в пределах предыдущей платформы, тогда выполняем смещение
        if ((Last - .75) <= current && current <= (Last + .75))
            current += -1.5f;

        Spawn.transform.position = new Vector3(current, transform.position.y + Random.Range(.3f, 1.2f));
        Last = current; //запомнаем положение текущей

        if (Spawn.transform.position.y < N)   
        {
            Instantiate(Platform, Spawn.transform.position, Spawn.transform.rotation);
        }
        else if (Spawn.transform.position.y < N + 10f)
        {
            float rand = Random.Range(-1, 1);
            GameObject obj = (rand == 0) ? MoveblePlatform : Platform;
            Instantiate(obj, Spawn.transform.position, Spawn.transform.rotation);
            Debug.Log(rand);
        }
        else if (Spawn.transform.position.y < N + 20f)
        {
            float rand = Random.Range(0, 3);
            GameObject obj;
            if (rand == 0)
                obj = InvisPlatform;
            else if (rand == 1)
                obj = MoveblePlatform;
            else
                obj = Platform;

            Instantiate(obj, Spawn.transform.position, Spawn.transform.rotation);
            Debug.Log(rand);
        }
        else if (Spawn.transform.position.y < N + 30f)
        {
            float rand = Random.Range(0, 3);
            GameObject obj;
            if (rand == 0)
                obj = InvisPlatform;
            else if (rand == 1)
                obj = MoveblePlatform;
            else if (rand == 2)
                obj = SpringPlatform;
            else
                obj = Platform;

            Instantiate(obj, Spawn.transform.position, Spawn.transform.rotation);
            Debug.Log(rand);
        }
        else
        {
            float rand = Random.Range(0, 5);
            GameObject obj;
            if (rand == 0)
                obj = InvisPlatform;
            else if (rand == 1)
                obj = MoveblePlatform;
            else if (rand == 2)
                obj = SpringPlatform;
            else if (rand == 3)
                obj = SlowPlatform;
            else
                obj = Platform;

            Instantiate(obj, Spawn.transform.position, Spawn.transform.rotation);
            Debug.Log(rand);
        }

    }
}
