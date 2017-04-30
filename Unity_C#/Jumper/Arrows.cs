using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrows : MonoBehaviour {

    private bool pressed = false;

    private void Update()
    {
        if (!pressed)
            return;
        else
        {
            if (gameObject.name == "ArrowL")
            {
                PlayerController.Instance.MoveLeft();
            }
            if (gameObject.name == "ArrowR")
            {
                PlayerController.Instance.MoveRight();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressed = false;
        }
    }

    private void OnMouseDown()
    {
        pressed = true;
    }

}
