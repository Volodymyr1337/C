using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{

    // крайние позиции
    float maxPosX = 0f;
    float maxPosZ = 0f;
    float minPosX = -15f;
    float minPosZ = -16f;

    bool zoomed = false;

    public float dragSpeed = 0.2f;
    private Vector3 dragOrigin;

    
    private void Update()
    {
        if (zoomed)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 5f, transform.position.z), 2f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(50f, 40f, 0),Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, 10f, transform.position.z), 2f * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(65f, 40f, 0), Time.deltaTime);
        }
    }

    private void LateUpdate()       // Передвижения камеры
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

        if (transform.position.x < minPosX && move.x < 0 || transform.position.x > maxPosX && move.x > 0)
            move.x = 0f;
        if (transform.position.z < minPosZ && move.z < 0 || transform.position.z > maxPosZ && move.z > 0)
            move.z = 0f;

        transform.Translate(move, Space.World);
    }

    public void Zooming(int i)
    {
        if (i == 1)
        {
            zoomed = true;
        }
        else
        {
            zoomed = false;
        }
    }
}
