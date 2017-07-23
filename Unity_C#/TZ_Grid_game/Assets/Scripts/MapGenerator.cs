using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform tilePrefab;
    public Vector2 mapSize;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()   // инстанциируем префабы с текстурой травы
    {
        for (int x = 0, j = 0; x < mapSize.x; x++)
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePos = new Vector3(-mapSize.x / 2 + x, 0, -mapSize.y/2 + y);
                Transform newTile = Instantiate(tilePrefab, tilePos, Quaternion.Euler(Vector3.right * 90)) as Transform;
                newTile.parent = transform;
                j++;
            }
    }
}
