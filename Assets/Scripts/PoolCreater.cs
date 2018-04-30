using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;


public class PoolCreater : MonoBehaviour
{


    private void Start()
    {

        CreatePool(20);
    }

    private void CreatePool(int num)
    {
        ResourcesManager.Instance.LoadMagic_sync();
        foreach (KeyValuePair<string, GameObject> go in ResourcesManager.Dict_Magic)
        {
            NetPoolManager.InitPrefab(go.Value, num);
        }

    }

}
