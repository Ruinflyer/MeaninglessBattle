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
    private Dictionary<string, Sprite> Dict_UITex;

    public static Dictionary<string, GameObject> Dict_Magic = new Dictionary<string, GameObject>();

    public string sceneName="";

    void Start()
    {
        //StartCoroutine(LoadMapTiles());
    }

    public IEnumerator LoadAllRes()
    {
        LoadUITextures();
        LoadMapTiles();
        LoadItems();
        LoadMagic();
        yield return true;
    }

    /// <summary>
    /// 加载UI图片
    /// </summary>
    public void LoadUITextures()
    {
        Dict_UITex = new Dictionary<string, Sprite>();
        BundleConf bundleConf = MeaninglessJson.LoadJsonFromFile<BundleConf>(Application.dataPath + "/StreamingAssets/" + "BC_UITextures.json");
        AssetBundle ab = AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/" + bundleConf.BundlePath);
        UnityEngine.U2D.SpriteAtlas spriteAtlas = ab.LoadAsset<UnityEngine.U2D.SpriteAtlas>("BagIcon");
        foreach (string str in bundleConf.ResName)
        {
            Dict_UITex.Add(str, spriteAtlas.GetSprite(str));
        }
    }
    public Sprite GetUITexture(string textureName)
    {
        Sprite resourceGObj = null;
        if (!Dict_UITex.TryGetValue(textureName, out resourceGObj))
        {
            Debug.LogError(textureName + "不在图集中");
        }

        if (resourceGObj == null)
        {
            Debug.LogError("无法获取");
        }
        return resourceGObj;
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
        GameObject gobj;
        if (!Dict_BaseMapTiles.TryGetValue(MapTileName, out gobj))
        {
            Debug.LogError(MapTileName + "不在文件夹中");
            return Dict_BaseMapTiles[MapTileName];
        }
        if (gobj == null)
        {
            Debug.LogError(MapTileName + " 无法获取");
        }
        return gobj;
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


    public IEnumerator LoadMagic()
    {
        Dict_Magic = new Dictionary<string, GameObject>();
        StartCoroutine(AssetBundleLoader.LoadBundleToDictAsync<GameObject>("BC_Magic.json", Dict_Magic));
        yield return true;
    }
    public void LoadMagic_sync()
    {
        Dict_Magic = new Dictionary<string, GameObject>();
        AssetBundleLoader.LoadBundleToDict<GameObject>("BC_Magic.json", Dict_Magic);
    }
    public GameObject GetMagic(string magicName)
    {
       
        GameObject resourceGObj = null;
        if (!Dict_Magic.TryGetValue(magicName, out resourceGObj))
        {
            Debug.LogError(magicName + "不在文件夹中");
        }

        if (resourceGObj == null)
        {
            Debug.LogError("无法获取");
        }
        return resourceGObj;
    }

    /// <summary>
    /// 加载场景assetbundl并且获得场景名字
    /// </summary>
    public void LoadSceneAndGetSceneName()
    {
        AssetBundle ab = AssetBundle.LoadFromFile(Application.dataPath + "/StreamingAssets/scene.ab");
        string[] scenePath = ab.GetAllScenePaths();
        string SceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath[0]);

        sceneName= SceneName;
    }

}
