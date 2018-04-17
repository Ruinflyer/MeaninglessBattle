using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using MeaninglessNetwork;



public class MapManager : MonoSingleton<MapManager>
{
    public bool isLoaded=false;

    //圈参数表
    private GameObject Circlefield;

    private ItemSpawnPoint ISP;
    private List<float> RandomList = new List<float>();
    private int ItemListIndex = 0;
    private int Seed;
    #region 物品变量
    private float[] ProbabilityValue;
    private int[] ItemsID;
    private float totalProbabilityValue = 0;
    private float getRandom = 0;
    #endregion
    // Use this for initialization
    void Start()
    {
        //LoadCirclefieldInfo();
        NetworkManager.ServerConnection.msgDistribution.AddEventListener("GetMapItemData", OnGetMapItemDataBack);
        NetworkManager.ServerConnection.msgDistribution.AddEventListener("Circlefield", OnCirclefieldBack);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /* DeadCode
    private void OnGetMapDataBack(BaseProtocol protocol)
    {
        BytesProtocol p = (BytesProtocol)protocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        MapData = new List<string>();
        int MapTileNum = p.GetInt(startIndex, ref startIndex);
        object[] param = new object[2];
        param[0] = "正在接收地图数据...";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);
        for (int i = 0; i < MapTileNum; i++)
        {
            MapData.Add(p.GetString(startIndex, ref startIndex));
        }
        param[0] = "地图数据接收完毕";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);
    }
    */
    /// <summary>
    /// 获取地图道具随机数种子
    /// </summary>
    private void OnGetMapItemDataBack(BaseProtocol protocol)
    {
        BytesProtocol p = (BytesProtocol)protocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        p.GetInt(startIndex, ref startIndex);
        Seed = p.GetInt(startIndex, ref startIndex);

        object[] param = new object[2];
        param[0] = "正在接收道具数据...";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);

        //220个物品生成点的随机数
        RandomList = InitTotalProbabilityValue(ItemInfoManager.Instance.GetTotalOccurrenceProbability());


        param[0] = "道具数据接收完毕";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);

        GenerateItem();
    }
    public void GenerateItem()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        //LoadingUI提示：
        object[] param = new object[2];
        param[0] = "正在生成地图物品...";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);


        //地上物品生成：

        for (int j = 0; j < ISP.ItemSpawnPoints.Length; j++)
        {
            Instantiate(ResourcesManager.Instance.GetItem(ItemInfoManager.Instance.GetItemName(ItemsID[CalcIndex(j,ItemInfoManager.Instance.GetTotalOccurrenceProbability())])),
                new Vector3(ISP.ItemSpawnPoints[j].position.x, 0, ISP.ItemSpawnPoints[j].position.z), Quaternion.identity);
        }



        param[0] = "地图物品生成完毕";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);
        isLoaded = true;
        yield return true;
    }

    /// <summary>
    /// 根据出现概率计算道具下标
    /// </summary>
    private int CalcIndex(int RandomListIndex,float[] probabilityValue)
    {

        
        for (int i = 0; i < probabilityValue.Length; i++)
        {
            if (RandomList[RandomListIndex] < probabilityValue[i])
            {
                return i;
            }
            else
            {
                RandomList[RandomListIndex] -= probabilityValue[i];
            }
        }
        return probabilityValue.Length - 1;

    }

    /// <summary>
    /// 返回一个浮点数列表,成员数为物品生成点数量
    /// </summary>
    /// <param name="probabilityValue"></param>
    /// <returns></returns>
    private List<float> InitTotalProbabilityValue(float[] probabilityValue)
    {
        if (totalProbabilityValue == 0)
        {
            for (int i = 0; i < probabilityValue.Length; i++)
            {
                totalProbabilityValue += probabilityValue[i];
            }
        }
        List<float> temp_RandomList = new List<float>();
        Random.InitState(Seed);
        for (int i = 0; i < ISP.ItemSpawnPoints.Length; i++)
        {
            temp_RandomList.Add(Random.Range(0, totalProbabilityValue));
        }
        return temp_RandomList;
    }
    public void OnCirclefieldBack(BaseProtocol protocol)
    {
        BytesProtocol p = (BytesProtocol)protocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        float X = p.GetFloat(startIndex, ref startIndex);
        float Y = p.GetFloat(startIndex, ref startIndex);
        float shrinkPercent = p.GetFloat(startIndex, ref startIndex);
        int Movetime = p.GetInt(startIndex, ref startIndex);

        iTween.ScaleTo(Circlefield, iTween.Hash("x", Circlefield.transform.localScale.x * shrinkPercent, "z", Circlefield.transform.localScale.z * shrinkPercent, "time", Movetime));
        iTween.MoveTo(Circlefield, iTween.Hash("position", new Vector3(X, 0, Y), "time", Movetime));
    }


}


