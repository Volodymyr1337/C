using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMovements : MonoBehaviour
{
    public bool pressed { get; set; }
    private int key;

    void Update()
    {
        if (!pressed)
        {
            return;
        }
        else
        {
            if (gameObject.name == "Left")
            {
                key = -1;
                User.Instance.Run(key);
            }
            if (gameObject.name == "Right")
            {
                key = 1;
                User.Instance.Run(key);
            }
            if (gameObject.name == "JumpBtn")
            {
                User.Instance.Jump();
            }
            if (gameObject.name == "SlideBtn")
            {
                User.Instance.Slide();
            }

            if (Input.GetMouseButtonUp(0))
            {
                User.Instance.checkLastMovements = User.Instance.checkMovements;
                pressed = false;
            }
        }
    }
    private void OnMouseDown()
    {
        pressed = true;
        
    }
}
