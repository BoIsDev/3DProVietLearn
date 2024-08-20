using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItem : MonoBehaviour
{
    public Dictionary<string,Queue<GameObject>> objPool = new Dictionary<string,Queue<GameObject>>();
    public Transform Holder;
    private static PoolItem instance;
    public static PoolItem Instance => instance;
   

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    protected GameObject CreateNewObj(GameObject obj)
    {
        GameObject newGo = Instantiate(obj,Holder);
        newGo.name = obj.name;
        return newGo;
    }

    public GameObject GetObjItem(GameObject gameObject)
    {
        if(objPool.TryGetValue(gameObject.name, out Queue<GameObject> objList))
        {
            if(objList.Count == 0)
            {
                return CreateNewObj(gameObject);
            }
            else
            {
                GameObject newObjList = objList.Dequeue();
                newObjList.SetActive(true);
                return newObjList;
            }
        }
        else
        {
           return CreateNewObj(gameObject);
        }
    }

    public void ReturnObjePool(GameObject gameObject)
    {
        if (objPool.TryGetValue(gameObject.name, out Queue<GameObject> objList))
        {
            objList.Enqueue(gameObject);
        }
        else
        {
            Queue<GameObject> newQueue = new Queue<GameObject>();
            newQueue.Enqueue(gameObject);
            objPool.Add(gameObject.name, newQueue);
        }
        gameObject.SetActive(false);

    }

}
