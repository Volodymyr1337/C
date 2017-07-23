using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Elements[] Buildings;    // массив построек с инфо
    public GameObject Storage;      // магазин построек
    public Text[] btnTxtCounter;

    public GameObject GridLight;    // сетка
    int coef;                       // вспомогательный коеф для вкл/выкл сетки

    public Transform Environment;

    // радиус и плотность застройки окружения
    public float envMinRadius, envMaxRadius;
    public int envDensity;              //  - осторожно, можно попасть в бесконечный цикл

    Ray ray;
    RaycastHit hit;

    [HideInInspector]
    public bool NotPlaceable = false;   // запрещает постройку в занятой ячейке

    
    void Awake()
    {
        if ((envMaxRadius - envMinRadius < 3f && envDensity > 14))
            Debug.Log("Бесконечный цикл!");
        else
            CreateEnvironment();
    }

    private void Start()
    {
        AutoScroll.Instance.AutoScrollItems();
        Storage.SetActive(false);
        GridLight.SetActive(false);

        // пул всех возможных построек
        for (int i = 0; i < Buildings.Length; i++)
        {
            PoolManager.Instance.CreatePool(Buildings[i].building, i, Buildings[i].objCount);
            btnTxtCounter[i].text = 0 + "/" + Buildings[i].objCount;
        }

    }

    void Update()
    {
        ShowBuildingInfo();
    }

    public void DisplayStorage()
    {
        Storage.SetActive(true);
        AutoScroll.Instance.AutoScrollItems();
    }

    #region Генерация позиций окружающей среды

    void CreateEnvironment()
    {
        Vector3 randPos = Vector3.zero;
        Vector3 center = new Vector3(-6f, 0f, -6f);

        int check = 0;          // кол-во пересекающих коллайдеров
        for (int n = 0; n < envDensity; n++)
            for (int i = 2; i < 7; i++)
            {
                do
                {
                    check = 0;
                    randPos = RandomCircle(center, Buildings[i].objSizeX, Buildings[i].objSizeZ, UnityEngine.Random.Range(envMinRadius, envMaxRadius));

                    // если по оси Х размер строения больше чем по Оу, то за радиус принимается размер по Оу
                    Collider[] hitColliders = Physics.OverlapSphere(randPos,
                        (Buildings[i].objSizeX > Buildings[i].objSizeZ) ? Buildings[i].objSizeX : Buildings[i].objSizeZ);

                    for (int j = 0; j < hitColliders.Length; j++)
                        if (hitColliders[j].tag == "Placed")
                            check++;

                } while (check > 0);
                GameObject go = Instantiate(Buildings[i].building, randPos, Quaternion.identity);
                go.transform.parent = Environment;
                Debug.Log(go.tag);
                go.tag = "Placed";
                Debug.Log(go.tag);
            }
    }
    
    Vector3 RandomCircle(Vector3 center, float sizeX, float sizeZ, float radius)
    {
        float ang = UnityEngine.Random.value * 360;
        float temp;

        Vector3 pos;

        temp = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad) + UnityEngine.Random.Range(1f, 6f);
        pos.x = (sizeX % 2 == 0) ? Mathf.Round(temp) : (Mathf.Round(temp) + 0.5f);

        pos.y = center.y;

        temp = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad) + UnityEngine.Random.Range(1f, 6f);
        pos.z = (sizeZ % 2 == 0) ? Mathf.Round(temp) : (Mathf.Round(temp) + 0.5f);

        return pos;
    }
    #endregion

    
    void ShowBuildingInfo()     // Вывод информации про объект
    {
        Vector3 pos;

        if (Input.touchCount == 0)          // ПК
            pos = Input.mousePosition;
        else
            pos = Input.GetTouch(0).position;

        ray = Camera.main.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out hit, 100f))                                            // ⬇ если не принадлежит к окружению ⬇
            if (hit.collider.gameObject.tag == "Placed" && Input.GetMouseButtonDown(0) && hit.collider.gameObject.transform.parent != Environment)
            {
                int index = hit.collider.gameObject.GetComponent<ObjMovements>().arrayIndex;
                Debug.Log("Имя: " + Buildings[index].ObjName + " | " +
                    "Id: " + Buildings[index].buildingId + " | " +
                    "Размер строения (" + Buildings[index].objSizeX + ", " + Buildings[index].objSizeX + ") | " +
                    "Макс количество: " + Buildings[index].objCount);
            }
    }

    public void Build(int iterator)
    {
        if (PoolManager.Instance.IfExist(iterator) == 0)    // если постройки даного типа закончились
        {
            btnTxtCounter[iterator].text = Buildings[iterator].objCount + "/" + Buildings[iterator].objCount;
            return;
        }
        
        Storage.SetActive(false);

        Vector3 newPos = Camera.main.transform.position + new Vector3(5f, 0f, 5f);     // позиция в которой появится постройка
        newPos.y = 0.5f;

        GameObject newBuilding = PoolManager.Instance.GetObject(iterator);
        newBuilding.transform.position = newPos;
        newBuilding.SetActive(true);
        newBuilding.GetComponentInChildren<ParticleSystem>().Play();
        ObjMovements newBuildingParams = newBuilding.GetComponent<ObjMovements>();

        newBuildingParams.arrayIndex = iterator;

        newBuildingParams.move = true;
        newBuildingParams.objSizeX = Buildings[iterator].objSizeX;
        newBuildingParams.objSizeZ = Buildings[iterator].objSizeZ;

        // Запись оставшегося кол-ва доступных построек
        btnTxtCounter[iterator].text = (Buildings[iterator].objCount - PoolManager.Instance.IfExist(iterator)) + "/" + Buildings[iterator].objCount;
    }

    public void EnableGrid(int k)    // вкл сетку
    {
        coef += k;

        if ((coef & 1) == 1)
            GridLight.SetActive(true);
        else
            GridLight.SetActive(false);
    }
}
[Serializable]
public class Elements
{
    public string ObjName;
    public GameObject building;
    public int buildingId;
    public float objSizeX, objSizeZ;
    public int objCount;
}
