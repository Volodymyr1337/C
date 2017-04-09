using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fonar_trigger : MonoBehaviour {

    public GameObject crow;
    private Animator flying;

    private bool check;

    private CrowState state
    {
        get { return (CrowState)flying.GetInteger("State"); }
        set { flying.SetInteger("State", (int)value); }
    }

    private void Start()
    {
        check = false;
        flying = crow.GetComponent<Animator>();
    }
    private void Update()
    {
        if (check)
        {
            state = CrowState.fly;
            crow.transform.position = Vector3.MoveTowards(crow.transform.position, new Vector3(crow.transform.position.x + 0.09f, crow.transform.position.y), 1f);
        }
        else
            state = CrowState.idle;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "User")
        {
            check = true;
        }
    }
}
public enum CrowState
{
    idle, fly
}
