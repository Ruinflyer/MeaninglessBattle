using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;


public class ObjectPoolManager : Mono_DDOLSingleton<ObjectPoolManager>
{
    public static Dictionary<int, ObjectPool> poolDictionary = new Dictionary<int, ObjectPool>();

    private static List<GameObject> prefabList = new List<GameObject>();

    private static Dictionary<GameObject, ObjectPool> objectPoolDict = new Dictionary<GameObject, ObjectPool>();

    public static ObjectPool CreateObjectPool(GameObject prefab, int num)
    {
        prefabList.Add(prefab);
        GameObject go = new GameObject();
        go.name = prefab.name + "Pool";
        ObjectPool objectPool = go.AddComponent<ObjectPool>();
        objectPool.InitObjectPool(prefab, objectPoolDict, num);
        poolDictionary.Add(prefab.GetInstanceID(), objectPool);
        return objectPool;
    }
    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public static ObjectPool CreateObjectPool(GameObject prefab, Vector3 position, Quaternion rotation, int num)
    {
        prefabList.Add(prefab);
        GameObject go = new GameObject();
        go.name = prefab.name + "Pool";
        ObjectPool objectPool = go.AddComponent<ObjectPool>();
        objectPool.InitObjectPool(prefab, position, rotation, objectPoolDict, num);
        return objectPool;
    }

    private static ObjectPool GetObjectPool(GameObject prefab, int num = 4)
    {
        ObjectPool objectPool = null;
        int prefabID = prefab.GetInstanceID();
        for (int i = 0; i < prefabList.Count; i++)
        {
            if (prefabID == prefabList[i].GetInstanceID())
            {
                objectPool = poolDictionary[prefabID];
                break;
            }
        }
        if (objectPool == null)
        {
            objectPool = CreateObjectPool(prefab, num);
        }
        return objectPool;
    }

    private static ObjectPool GetObjectPool(GameObject prefab, Vector3 position, Quaternion rotation, int num = 4)
    {
        ObjectPool objectPool = null;
        int prefabID = prefab.GetInstanceID();
       
        for (int i = 0; i < prefabList.Count; i++)
        {
            if (prefabID == prefabList[i].GetInstanceID())
            {
                objectPool = poolDictionary[prefabID];
                break;
            }
        }
        if (objectPool == null)
        {
            objectPool = CreateObjectPool(prefab, position, rotation, num);
        }
        return objectPool;
    }

    public static void InitPrefab(GameObject prefab, int initNum = 4)
    {
        GetObjectPool(prefab, initNum);
    }

    public static Transform PullObjcetFromPool(GameObject prefab)
    {
        if (prefab == null)
        {
            return null;
        }
        ObjectPool objectPool = GetObjectPool(prefab);

        return objectPool.PullObject();
    }
    public static Transform PullObjcetFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
        {
            return null;
        }
        ObjectPool objectPool = GetObjectPool(prefab, position, rotation);
       

        return objectPool.PullObject(position, rotation);
    }

    public static void PushObjectFromPool(GameObject prefab, GameObject handleObject)
    {
        ObjectPool objectPool = GetObjectPool(prefab);
        objectPool.PushObject(handleObject);
    }

    public static void PushObjectFromPool(GameObject handleObject)
    {
        ObjectPool objectPool = GetObjectPoolByObject(handleObject);
        if (objectPool)
            objectPool.PushObject(handleObject);
        else
            GameObject.Destroy(handleObject);
    }

    private static ObjectPool GetObjectPoolByObject(GameObject handleObject)
    {
        if (objectPoolDict.ContainsKey(handleObject))
        {
            return objectPoolDict[handleObject];
        }
        Debug.LogError("找不到对象池");
        return null;
    }
}
