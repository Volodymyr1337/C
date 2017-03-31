using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {



    private void OnMouseUpAsButton()
    {
        if (gameObject.name == "Exit")
            Application.Quit();
    }
}
