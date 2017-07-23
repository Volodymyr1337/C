using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// где надо создать пул подставляем
// PoolManager.Instance.CreatePool(...);

public class PoolManager : MonoBehaviour
{
    Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();
    
    static PoolManager instance;


    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PoolManager>();
            }
            return instance;
        }
    }
        
    public void CreatePool(GameObject prefab, int key, int count)    // key - индекс в массиве префабов
    {

        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary.Add(key, new Queue<GameObject>());

            for (int i = 0; i < count; i++)
            {
                GameObject newObject = Instantiate(prefab) as GameObject;
                newObject.GetComponent<Transform>().parent = transform;
                newObject.SetActive(false);
                poolDictionary[key].Enqueue(newObject);
            }
        }
    }
    public int IfExist(int i)       // Для проверки наличия и кол-ва объектов одного типа
    {
        return poolDictionary[i].Count;        
    }
    
    public GameObject GetObject(int index)              // Извлекаем из пула
    {
        return poolDictionary[index].Dequeue();
    }

    
    public void ReturnObject(int index, GameObject go)  // Возврат в пул
    {
        go.SetActive(false);
        poolDictionary[index].Enqueue(go);
    }



}
