﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using MeaninglessNetwork;



public class MapManager : MonoSingleton<MapManager>
{
    public bool isLoaded = false;

    //圈参数表
    private GameObject Circlefield;

    public ItemSpawnPoint itemSpawnPoint;
    private List<float> RandomList = new List<float>();
    private int ItemListIndex = 0;
    private int Seed=0;

    /// <summary>
    /// 门字典 ID,Transform
    /// </summary>
    public Dictionary<int, Transform> Doors = new Dictionary<int, Transform>();

    /// <summary>
    /// 地面物件字典,GroundItemID唯一标识
    /// </summary>
    public Dictionary<int, GroundItem> Items = new Dictionary<int, GroundItem>();
    #region 物品变量
    private float[] ProbabilityValue=null;
    private int[] ItemsID =null;
    private float totalProbabilityValue = 0;
   
    #endregion
    // Use this for initialization
    void Start()
    {
        itemSpawnPoint = GetComponent<ItemSpawnPoint>();
        //LoadCirclefieldInfo();
        //初始化网络事件
        NetworkManager.ServerConnection.msgDistribution.AddEventListener("GetMapItemData", OnGetMapItemDataBack);
        NetworkManager.ServerConnection.msgDistribution.AddEventListener("Circlefield", OnCirclefieldBack);
        NetworkManager.ServerConnection.msgDistribution.AddEventListener("DoorOpen", OnDoorOpen);
        NetworkManager.ServerConnection.msgDistribution.AddEventListener("AllPlayerLoaded", OnAllPlayerLoaded);
        
        //门加入字典
        for (int i=0;i<itemSpawnPoint.DoorSpawnPoints.Length;i++)
        {
            Doors.Add(i,itemSpawnPoint.DoorSpawnPoints[i]);
        }

        ProbabilityValue = ItemInfoManager.Instance.GetTotalOccurrenceProbability();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 请求获得道具数据
    /// </summary>
    public void RequestItemData()
    {
        BytesProtocol p = new BytesProtocol();
        p.SpliceString("GetMapItemData");
        NetworkManager.ServerConnection.Send(p);
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
        Seed = p.GetInt(startIndex, ref startIndex);

        object[] param = new object[2];
        param[0] = "正在接收道具数据...";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);

        if(Seed!=0)
        {
            //220个物品生成点的随机数
            RandomList = InitTotalProbabilityValue(ProbabilityValue);
            param[0] = "道具数据接收完毕";
            param[1] = 2;
            MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);

            GenerateItem();
        }
        else
        {
            Debug.LogError("Seed异常 "+Seed.ToString()+" 无法加载");
        }
    }
    //生成地面道具
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
        GameObject tmp = null;
        int tmp_ID = 0;
        GroundItem tmp_groundItem;
        for (int j = 0; j < itemSpawnPoint.ItemSpawnPoints.Length; j++)
        {
            tmp_ID = ItemsID[CalcIndex(j, ProbabilityValue)];
            tmp =Instantiate(ResourcesManager.Instance.GetItem(ItemInfoManager.Instance.GetItemName(tmp_ID)),
                new Vector3(itemSpawnPoint.ItemSpawnPoints[j].position.x, 0, itemSpawnPoint.ItemSpawnPoints[j].position.z), Quaternion.identity);
            tmp_groundItem = tmp.AddComponent<GroundItem>();
            tmp_groundItem.ItemID = tmp_ID;
            tmp_groundItem.GroundItemID = j;
            Items.Add(j, tmp_groundItem);
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
        for (int i = 0; i < itemSpawnPoint.ItemSpawnPoints.Length; i++)
        {
            
            temp_RandomList.Add(Random.Range(0, totalProbabilityValue));
        }
        return temp_RandomList;
    }

    /// <summary>
    /// 毒圈回调
    /// </summary>
    /// <param name="protocol"></param>
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

    /// <summary>
    /// 开门
    /// </summary>
    /// <param name="protocol"></param>
    public void OnDoorOpen(BaseProtocol protocol)
    {
        BytesProtocol p = (BytesProtocol)protocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int DoorID = p.GetInt(startIndex, ref startIndex);
        if(itemSpawnPoint.DoorSpawnPoints[DoorID]!=null)
        {
            Doors[DoorID].gameObject.GetComponent<DoorControl>().ControlDoor();
        }
    }

    /// <summary>
    /// 所有人加载完毕后的处理
    /// </summary>
    /// <param name="protocol"></param>
    public void OnAllPlayerLoaded(BaseProtocol protocol)
    {

    }
}


