using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using MeaninglessNetwork;



public class MapManager : MonoSingleton<MapManager>
{
    public bool isLoaded = false;

    #region 毒圈变量
    private GameObject Circlefield;
    public float countdownTime = 0;
    private float lastTime=0;
    public bool Moving = false;
    private bool isBegin = false;
    public Vector3 CircleCenter=Vector3.zero;
    private Chunky chunkyEffect;
    #endregion
    //下降点
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
        Circlefield = GameObject.Find("CircleField");
        itemSpawnPoint = GetComponent<ItemSpawnPoint>();
        //LoadCirclefieldInfo();
        //初始化网络事件
        NetworkManager.AddEventListener("GetMapItemData", OnGetMapItemDataBack);
        NetworkManager.AddEventListener("Circlefield", OnCirclefieldBack);
        NetworkManager.AddEventListener("CirclefieldTime",OnCirclefieldTimeBack);
        NetworkManager.AddEventListener("DoorOpen", OnDoorOpen);
        NetworkManager.AddEventListener("AllPlayerLoaded", OnAllPlayerLoaded);
        NetworkManager.AddEventListener("PickItem",OnPickItem);
        NetworkManager.AddEventListener("DropItem", OnDropItem);
       
        //门加入字典
        for (int i=0;i<itemSpawnPoint.DoorSpawnPoints.Length;i++)
        {
            itemSpawnPoint.DoorSpawnPoints[i].GetComponent<DoorControl>().DoorID = i;
            Doors.Add(i,itemSpawnPoint.DoorSpawnPoints[i]);
        }

        ProbabilityValue = ItemInfoManager.Instance.GetTotalOccurrenceProbability();
        ItemsID = ItemInfoManager.Instance.GetAllItemsID();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastTime > 1f && MapManager.Instance.countdownTime > 0f)
        {
           
            MapManager.Instance.countdownTime -= 1;
            lastTime = Time.time;
            float d = Vector3.Distance(CameraBase.Instance.player.transform.position, Circlefield.transform.position);
            //超出半径时
            if (chunkyEffect == null)
            {
                chunkyEffect = Camera.main.gameObject.GetComponent<Chunky>();
            }
            if(isBegin)
            {
                //Debug.Log(d.ToString()+"  "+ Circlefield.transform.localScale.x);
                if (d > Circlefield.transform.localScale.x)
                {
                    if (chunkyEffect.enabled == false)
                    {
                        chunkyEffect.enabled = true;
                        AudioManager.PlayMusic2D("Poison",true).Play();
                    }
                    //发送毒圈伤害消息
                    NetworkManager.SendPlayerPoison();
                }
                else
                {
                    if (chunkyEffect.enabled == true)
                    {
                        chunkyEffect.enabled = false;
                        AudioManager.PlayMusic2D("Poison", true).Pause();
                    }

                }
            }
           
        }

    }


    #region 生成地面道具代码

    /// <summary>
    /// 请求获得道具数据
    /// </summary>
    public void RequestItemData()
    {
        BytesProtocol p = new BytesProtocol();
        p.SpliceString("GetMapItemData");
        NetworkManager.Send(p);
    }

    /// <summary>
    /// 获取地图道具随机数种子
    /// </summary>
    private void OnGetMapItemDataBack(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        Seed = p.GetInt(startIndex, ref startIndex);

        object[] param = new object[2];
        param[0] = "正在接收道具数据...";
        param[1] = 0;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);

        if (Seed != 0)
        {
            //220个物品生成点的随机数
            RandomList = InitTotalProbabilityValue(ProbabilityValue);
            param[0] = "道具数据接收完毕";
            param[1] = 0;
            MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);

            GenerateItem();
        }
        else
        {
            Debug.LogError("Seed异常 " + Seed.ToString() + " 无法加载");
        }
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
        param[1] =0;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);


        //地上物品生成：
        GameObject tmp = null;
        int tmp_ID = 0;
        GroundItem tmp_groundItem;
        
        for (int j = 0; j < itemSpawnPoint.ItemSpawnPoints.Length; j++)
        {
            tmp_ID = ItemsID[CalcIndex(j, ProbabilityValue)];
            //Debug.LogError("Index: "+j +" ID: "+tmp_ID+" ResName: "+"ResName: "+ItemInfoManager.Instance.GetResname(tmp_ID) +" ItemName: "+ ItemInfoManager.Instance.GetItemName(tmp_ID));
            tmp =Instantiate(ResourcesManager.Instance.GetItem(ItemInfoManager.Instance.GetResname(tmp_ID)),
                new Vector3(itemSpawnPoint.ItemSpawnPoints[j].position.x, 0, itemSpawnPoint.ItemSpawnPoints[j].position.z), Quaternion.Euler(new Vector3(-90,0,0)));
            tmp_groundItem = tmp.AddComponent<GroundItem>();
            tmp_groundItem.ItemID = tmp_ID;
            tmp_groundItem.GroundItemID = j;
            //tmp.transform.SetParent(itemSpawnPoint.ItemSpawnPoints[j]);
            Items.Add(j, tmp_groundItem);
        }

        param[0] = "地图物品生成完毕";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);
        NetworkManager.SendPlayerReady();
        NetworkManager.SendReqDroppoint();

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

    #endregion
    #region 毒圈回调代码
    /// <summary>
    /// 毒圈回调
    /// </summary>
    /// <param name="protocol"></param>
    public void OnCirclefieldBack(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        float X = p.GetFloat(startIndex, ref startIndex);
        float Y = p.GetFloat(startIndex, ref startIndex);
        float shrinkPercent = p.GetFloat(startIndex, ref startIndex);
        int Movetime = p.GetInt(startIndex, ref startIndex);
        //HUDUI倒计时显示
        countdownTime = Movetime;
        Moving = true;
        CircleCenter = new Vector3(X,0,Y);

        isBegin = true;
        iTween.ScaleTo(Circlefield, iTween.Hash("x", Circlefield.transform.localScale.x*shrinkPercent, "y", Circlefield.transform.localScale.y*shrinkPercent, "time", Movetime, "easeType",iTween.EaseType.linear));
        iTween.MoveTo(Circlefield, iTween.Hash("position", new Vector3(X, 0, Y), "time", Movetime, "easeType", iTween.EaseType.linear));
    }
    public void OnCirclefieldTimeBack(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int HoldTime = p.GetInt(startIndex, ref startIndex);
        //HUDUI倒计时显示
        countdownTime = HoldTime;
        Moving = false;
    }
    #endregion
    #region 地图物件被使用回调代码
    /// <summary>
    /// 开门
    /// </summary>
    /// <param name="protocol"></param>
    public void OnDoorOpen(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int DoorID = p.GetInt(startIndex, ref startIndex);
        if(itemSpawnPoint.DoorSpawnPoints[DoorID]!=null)
        {
            Doors[DoorID].gameObject.GetComponent<DoorControl>().ControlDoor();
        }
    }
    /// <summary>
    /// 游戏物品被拾取处理
    /// </summary>
    /// <param name="protocol"></param>
    public void OnPickItem(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int GroundItemID = p.GetInt(startIndex, ref startIndex);
        if (Items.ContainsKey(GroundItemID))
        {
            Items[GroundItemID].Hide();
        }

    }

    /// <summary>
    /// 所有人加载完毕后的处理
    /// </summary>
    /// <param name="protocol"></param>
    public void OnAllPlayerLoaded(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        UIManager.Instance.HideTheUI(UIid.LoginUI, delegate { });
        UIManager.Instance.HideTheUI(UIid.MainUI, delegate { });
        UIManager.Instance.HideTheUI(UIid.RoomUI, delegate { });
        UIManager.Instance.HideTheUI(UIid.LoadingUI, delegate { });
        UIManager.Instance.ShowUI(UIid.HUDUI);

        NetworkManager.SendGetPlayersInfo();
    }



    /// <summary>
    /// 接收到物品被扔消息
    /// </summary>
    public void OnDropItem(BaseProtocol protocol)
    {
        BytesProtocol p = protocol as BytesProtocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        int GroundItemID = p.GetInt(startIndex, ref startIndex);
        float posX= p.GetFloat(startIndex, ref startIndex);
        float posY = p.GetFloat(startIndex, ref startIndex);
        float posZ = p.GetFloat(startIndex, ref startIndex);
        if (Items.ContainsKey(GroundItemID))
        {
            Items[GroundItemID].Show();
            Items[GroundItemID].transform.position = new Vector3(posX, posY, posZ);
        }
    }

    #endregion
}


