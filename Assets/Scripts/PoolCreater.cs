using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public struct PoolObject
{
    public GameObject go;
    public int num;
}

public class PoolCreater : MonoBehaviour
{


    public List<PoolObject> poolObjectlist = new List<PoolObject>();

    private void Start()
    {
        StartCoroutine(LoadResources());
    }

    IEnumerator LoadResources()
    {
        yield return new WaitForEndOfFrame();

        PoolObject poolObject;
        List<string> list = new List<string>();
        foreach (string key in ResourcesManager.Dict_Magic.Keys)
        {
            list.Add(key);
        }
        for (int i = 0; i < ResourcesManager.Dict_Magic.Count; i++)
        {
            poolObject.go = ResourcesManager.Instance.GetMagic(list[i]);
            poolObject.num = 50;
            poolObjectlist.Add(poolObject);
        }

        foreach (PoolObject po in poolObjectlist)
        {
            NetPoolManager.InitPrefab(po.go, po.num);
            Debug.Log("已建立" + po.go.name + "池");
        }
    }

}
