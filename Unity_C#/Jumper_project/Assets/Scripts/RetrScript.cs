﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetrScript : MonoBehaviour {

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene(0);
    }
}
