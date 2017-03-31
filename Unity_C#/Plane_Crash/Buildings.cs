using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Buildings : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Plane>() != null)
            GameController.instance.PlaneScored();
    }

}
