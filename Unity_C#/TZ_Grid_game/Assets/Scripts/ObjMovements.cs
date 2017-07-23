using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMovements : MonoBehaviour
{
    private float speed = 15f;
    private float x, y, z;

    GameManager gm;

    [HideInInspector]
    public bool move = false;           // флаг доступности перемещения
    [HideInInspector]
    public float objSizeX, objSizeZ;    // размеры объекта в ячейках
    [HideInInspector]
    public int arrayIndex;              // порядковый номер в массиве построек

	void Start ()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.NotPlaceable = false;
        x = 0f;
        y = 0f;
        z = 0f;
    }
	
	void Update ()
    {
        // передвижения построек осуществляются зажатой ЛКМ или тачем 
        // (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        if (move && Input.GetMouseButton(0))
        {
            Vector3 pos;

            if (Input.touchCount == 0)          // если с ПК
                pos = Input.mousePosition;
            else
                pos = Input.GetTouch(0).position;

            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, 100f))
            {
                x = (objSizeX % 2 == 0)? Mathf.Round(hit.point.x) : (Mathf.Round(hit.point.x) + 0.5f);
                z = (objSizeZ % 2 == 0) ? Mathf.Round(hit.point.z) : (Mathf.Round(hit.point.z) + 0.5f);
                transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, z), speed * Time.deltaTime);
            }  
        }
        // отпуская тач/мышь - устанавливается постройка (при условии, что коллайдеры не пересекаются) 
        // ((Input.touchCount > 0 && Input.GetTouch(Input.touchCount - 1).phase == TouchPhase.Ended)
        if (!gm.NotPlaceable && move && Input.GetMouseButtonUp(0))
        {
            transform.position = new Vector3(x, y, z);
            transform.tag = "Placed";
            move = false;
            GetComponentInChildren<ParticleSystem>().Stop();
            AudioPlayer.Instance.PlayConstractionSound();
        }
        else if (gm.NotPlaceable && move && Input.GetMouseButtonUp(0))  // текущую постройку возвращаем в пул и разрешаем строить новые 
        {
            gm.NotPlaceable = false;    
            GetComponentInChildren<ParticleSystem>().startColor = Color.green;
            PoolManager.Instance.ReturnObject(arrayIndex, gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {

        if (col.tag == "building")
        {
            gm.NotPlaceable = true;
            col.GetComponentInChildren<ParticleSystem>().startColor = Color.red;
        }
    }
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "building")
        {
            gm.NotPlaceable = true;
            col.GetComponentInChildren<ParticleSystem>().startColor = Color.red;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "building")
        {
            gm.NotPlaceable = false;
            col.GetComponentInChildren<ParticleSystem>().startColor = Color.green;
        }
    }
}
