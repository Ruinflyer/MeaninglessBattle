using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class ResourcesManager : Mono_DDOLSingleton<ResourcesManager>
{
    /*
    public static readonly string Path =
#if UNITY_ANDROID
    "jar:file://" + Application.dataPath + "!/assets/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
    "file://" + Application.dataPath + "/StreamingAssets/";
#else
            string.Empty;  
#endif
*/

    private Dictionary<string, GameObject> Dict_BaseMapTiles;
    private Dictionary<string, GameObject> Dict_Items;
    void Start()
    {
        //StartCoroutine(LoadMapTiles());
    }


    public IEnumerator LoadMapTiles()
    {
        if (Dict_BaseMapTiles == null)
        {
            Dict_BaseMapTiles = new Dictionary<string, GameObject>();
            StartCoroutine(AssetBundleLoader.LoadBundleToDictAsync("BC_Map.json", Dict_BaseMapTiles));

            yield return true;
        }
    }

    public GameObject GetMapTiles(string MapTileName)
    {
        if (Dict_BaseMapTiles != null)
        {
            return Dict_BaseMapTiles[MapTileName];
        }
        return null;
    }

    public IEnumerator LoadItems()
    {
        if (Dict_Items == null)
        {
            Dict_Items = new Dictionary<string, GameObject>();
            StartCoroutine(AssetBundleLoader.LoadBundleToDictAsync<GameObject>("BC_Items.json", Dict_Items));
            yield return true;
        }
    }
    public void LoadItems_sync()
    {
        Dict_Items = new Dictionary<string, GameObject>();
        AssetBundleLoader.LoadBundleToDict<GameObject>("BC_Items.json", Dict_Items);
    }
    public GameObject GetItem(string itemResname)
    {
        GameObject tmp;
        if (Dict_Items != null)
        {
            Dict_Items.TryGetValue(itemResname, out tmp);
            return Dict_Items[itemResname];
        }
        return null;
    }
}
