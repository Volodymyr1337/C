using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Create an array of buildings with spawn rate 4s
 */

public class BuildingPool : MonoBehaviour {

    public int buildingPoolSize = 5;
    public GameObject buildingPrefab;
    public float spawnRate = 4f;
    private bool gameStart = true;
    public float buildingMin = -3f;
    public float buildingMax = 1f;

    private GameObject[] buildings;
    private Vector2 objectPoolPosition = new Vector2(-15f, -25f);
    private float timeSinceLastSpawned;
    private float spawnXPosition = 15f;
    private int currentBuilding = 0;

	// Use this for initialization
	void Start ()
    {
        buildings = new GameObject[buildingPoolSize];
		for (int i = 0; i < buildingPoolSize; i++)
        {
            buildings[i] = Instantiate(buildingPrefab, objectPoolPosition, Quaternion.identity) as GameObject;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (!GameController.instance.gameOver && (timeSinceLastSpawned >= spawnRate || gameStart))
        {
            gameStart = false;
            timeSinceLastSpawned = 0;
            float spawnYPosition = Random.Range(buildingMin, buildingMax);
            buildings[currentBuilding].transform.position = new Vector2(spawnXPosition, spawnYPosition);
            currentBuilding++;
            if (currentBuilding >= buildingPoolSize)
            {
                currentBuilding = 0;
            }
        }
	}
}
