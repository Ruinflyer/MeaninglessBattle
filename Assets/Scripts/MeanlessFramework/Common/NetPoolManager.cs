using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using System;

public class NetPoolManager : ObjectPoolManager
{
    public void Awake()
    {
        //PhotonNetwork.PrefabPool = this;      
    }

    public static GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        GameObject go = null;

        if(ResourcesManager.Instance.GetMagic(prefabId)!=null)
        {
            go = ResourcesManager.Instance.GetMagic(prefabId);
        }
        else if(ResourcesManager.Instance.GetItem(prefabId)!=null)
        {
            go = ResourcesManager.Instance.GetItem(prefabId);
        }
        else if(ResourcesManager.Instance.GetMapTiles(prefabId) != null)
        {
            go = ResourcesManager.Instance.GetMapTiles(prefabId);
        }
        return ObjectPoolManager.PullObjcetFromPool(go, position, rotation).gameObject;
    }


    public static void Destroy(GameObject gameObject)
    {
        ObjectPoolManager.PushObjectFromPool(gameObject);
        //gameObject.DestroyToPool(gameObject);
    }

 
}
