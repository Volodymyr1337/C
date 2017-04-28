using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    private GameObject User;
    private Collider2D col;

    private void Start()
    {
        User = GameObject.FindGameObjectWithTag("Player");
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        
        if (gameObject != null)
        {
            if ((User.transform.position.y - 10f) > transform.position.y)   //удаляем платформы ниже юзера на 10 ед.
            {
                Destroy(gameObject);
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (col)
        {
            //если юзер ниже платформы - выкл коллайдер
            if (User.transform.position.y > transform.position.y - .2f)
                col.enabled = true;
            else
                col.enabled = false;
        }
    }
}
