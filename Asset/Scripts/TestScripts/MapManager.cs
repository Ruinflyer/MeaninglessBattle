using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using Photon;

public class MapManager : MonoSingleton<MapManager>
{
    private float X = 0;
    private float Z = 0;

    private int MoutainRow;
    private int MoutainCol;
    private int LakeRow;
    private int LakeCol;
    private int[] WildHouseRow;
    private int[] WildHouseCol;
    private int[] WildtileCounter;
    private List<int> WildTileFullList;
    private const int WildHouseamount = 4;
    private int[] TownareaRow;
    private int[] TownareaCol;
    private const int Townamount = 3;

    private PhotonView pv;
    private GameObject tmp_Maptile;
    private ItemSpawnPoint ISP;

    private string[][] MapData;

    private float[] ProbabilityValue;
    private int[] ItemsID;
    private float totalProbabilityValue = 0;
    private float getRandom = 0;
    private string tmp_itemname;

    private GameObject Circlefield;

    //圈参数表
    List<SingleCirclefield> SCFs;

    // Use this for initialization

    [System.Serializable]
    public class MaptileInfo
    {
        public string[] Town_Resname;
        public string[] Lake_Resname;
        public string[] Wild_Resname;
        public string[] Forest_Resname;
        public string[] Wildhouse_Resname;
        public string[] Moutain_Resname;
    }

    [System.Serializable]
    public class CirclefieldInfo
    {
        public List<SingleCirclefield> Circlefields;
    }

    //单个圈参数
    [System.Serializable]
    public class SingleCirclefield
    {
        //圈持续时间
        public int Holdtime;
        //圈移动时间
        public int Movetime;
        //收缩为原圈半径的比例
        public float ShrinkPercent;
        //处在此阶段的圈时对玩家造成每秒伤害
        public int DamagePerSec;

    }


    void Start()
    {
        LoadCirclefieldInfo();
        pv = NetworkCenter.Instance.GetGlobalPhotonview();
    }

    // Update is called once per frame
    void Update()
    {
    }


    /// <summary>
    /// 生成素质广场
    /// </summary>
    public bool GenerateSquare()
    {

        GameObject square = Instantiate(ResourcesManager.Instance.GetMapTiles("Square"), new Vector3(-50, 50, -50), Quaternion.identity);
        square.transform.parent = gameObject.transform;
        if (square != null)
        {
            return true;
        }


        return false;
    }

    public void GenerateMap()
    {
        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(MasterClientGenerate());
        }

    }

    /// <summary>
    /// 生成地图区域数据
    /// </summary>
    void SpawnAreapoint()
    {
        MaptileInfo maptile = new MaptileInfo();
        maptile = MeaninglessJson.LoadJsonFromFile<MaptileInfo>(MeaninglessJson.Path_StreamingAssets + "MaptileInfo.json");

        //实例化MapData
        MapData = new string[8][];
        for (int k = 0; k < 8; k++)
        {
            MapData[k] = new string[8];
        }


        Random.InitState(Random.Range(1, 9999));
        MoutainCol = Random.Range(0, 7);
        MoutainRow = Random.Range(0, 7);
        Debug.Log("Moutain:(" + MoutainCol + "," + MoutainRow + ")");

        /* 在范围内即生成山，不在则不生成
         * 山被森林围住-占3x3格地块
         */
        if (MoutainCol > 0 && MoutainCol < 7)
        {
            if (MoutainRow > 0 && MoutainRow < 7)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        MapData[MoutainCol - 1 + i][MoutainRow - 1 + j] = maptile.Forest_Resname[Random.Range(0, maptile.Forest_Resname.Length)];
                    }
                }
                MapData[MoutainCol][MoutainRow] = maptile.Moutain_Resname[Random.Range(0, maptile.Moutain_Resname.Length)];
            }
        }

        Random.InitState(Random.Range(1, 9999));
        //生成湖
        do
        {
            LakeCol = Random.Range(1, 6);
            LakeRow = Random.Range(1, 6);
            Debug.Log("Lake : (" + LakeCol + "," + LakeRow + ")");
        }
        while (Mathf.Abs((LakeCol - MoutainCol)) < 2 || Mathf.Abs((LakeRow - MoutainRow)) < 2);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                MapData[LakeCol - 1 + i][LakeRow - 1 + j] = maptile.Forest_Resname[Random.Range(0, maptile.Forest_Resname.Length)];
            }
        }
        MapData[LakeCol][LakeRow] = maptile.Lake_Resname[Random.Range(0, maptile.Lake_Resname.Length)];

        //生成城镇
        TownareaCol = new int[Townamount];
        TownareaRow = new int[Townamount];
        for (int i = 0; i < Townamount; i++)
        {
            TownareaCol[i] = Random.Range(1, 5);
            TownareaRow[i] = Random.Range(1, 5);
            Debug.Log("Townarea" + i + " : (" + TownareaCol[i] + "," + TownareaRow[i] + ")");
        }


        for (int i = 0; i < Townamount; i++)
        {
            for (int j = 0; j < Townamount; j++)
            {
                MapData[TownareaCol[i]][TownareaRow[i] + j] = maptile.Town_Resname[Random.Range(0, maptile.Town_Resname.Length)];

            }
        }

        //生成野外房屋
        WildHouseCol = new int[WildHouseamount];
        WildHouseRow = new int[WildHouseamount];
        for (int i = 0; i < WildHouseamount; i++)
        {
            WildHouseCol[i] = Random.Range(0, 7);
            WildHouseRow[i] = Random.Range(0, 7);
            Debug.Log("WildHouse" + i + " : (" + WildHouseCol[i] + "," + WildHouseRow[i] + ")");
        }
        for (int i = 0; i < WildHouseamount; i++)
        {
            Random.InitState(Random.Range(1, 9999));
            MapData[WildHouseCol[i]][WildHouseRow[i]] = maptile.Wildhouse_Resname[Random.Range(0, maptile.Wildhouse_Resname.Length)];
        }


        //生成野外草地

        int m = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (MapData[i][j] == null)
                {
                    Random.InitState(Random.Range(1, 9999));
                    m = Random.Range(0, maptile.Wild_Resname.Length);
                    MapData[i][j] = maptile.Wild_Resname[m];

                }
            }
        }
    }

    IEnumerator MasterClientGenerate()
    {
        InitProbabilityValue();
        SpawnAreapoint();

        if (MapData.Length != 0)
        {
            Instantiate(ResourcesManager.Instance.GetMapTiles("Edge"), Vector3.zero, Quaternion.identity).transform.parent = gameObject.transform;
            pv.RPC("GenerateMapTileOnNetwork", PhotonTargets.Others, "Edge", Vector3.zero);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    tmp_Maptile = Instantiate(ResourcesManager.Instance.GetMapTiles(MapData[i][j]), new Vector3(X, 0, Z), Quaternion.identity);
                    ISP = tmp_Maptile.GetComponent<ItemSpawnPoint>();
                    tmp_Maptile.transform.parent = gameObject.transform;
                    pv.RPC("GenerateMapTileOnNetwork", PhotonTargets.Others, MapData[i][j], new Vector3(X, 0, Z));
                    if (ISP != null)
                    {
                        if (ISP.DoorSpawnPoints.Length != 0)
                        {
                            for (int k = 0; k < ISP.DoorSpawnPoints.Length; k++)
                            {
                                pv.RPC("SyncDoorID", PhotonTargets.Others, k, PhotonNetwork.AllocateViewID());
                            }
                        }

                        if (ISP.ItemSpawnPoints.Length != 0)
                        {
                            for (int l = 0; l < ISP.ItemSpawnPoints.Length; l++)
                            {
                                //概率化物品名
                                tmp_itemname = ItemInfoManager.Instance.GetResname(ItemsID[CalcIndex(ProbabilityValue)]);
                                Instantiate(ResourcesManager.Instance.GetItem(tmp_itemname.ToString()), ISP.ItemSpawnPoints[l].position, Quaternion.identity);
                                pv.RPC("SpawnItem", PhotonTargets.Others, ISP.ItemSpawnPoints[l].position, Quaternion.identity, tmp_itemname, PhotonNetwork.AllocateViewID());
                            }
                        }
                    }


                    X += 50;
                    if (j == 7)
                    {
                        Z += 50;
                        X = 0;
                    }
                }
            }
            Circlefield = Instantiate(ResourcesManager.Instance.GetMapTiles("CircleField"), Vector3.zero, Quaternion.identity);
            pv.RPC("GenerateMapTileOnNetwork", PhotonTargets.Others, "CircleField", Vector3.zero);
            StartCirclefield();
        }
        yield return true;
    }



    #region 道具概率生成代码

    /// <summary>
    /// 初始化概率值数据
    /// </summary>
    private void InitProbabilityValue()
    {
        ItemInfoManager.Instance.LoadInfo();
        if (ProbabilityValue == null)
        {
            //所有物品的获得概率数组
            ProbabilityValue = ItemInfoManager.Instance.GetTotalOccurrenceProbability();
        }
        if (ItemsID == null)
        {
            //所有物品的ID数组
            ItemsID = ItemInfoManager.Instance.GetAllItemsID();
        }
    }
    /// <summary>
    /// 计算下标
    /// </summary>
    /// <param name="probabilityValue">所有物品的获得概率数组</param>
    /// <returns></returns>
    private int CalcIndex(float[] probabilityValue)
    {

        if (totalProbabilityValue == 0)
        {
            for (int i = 0; i < probabilityValue.Length; i++)
            {
                totalProbabilityValue += probabilityValue[i];
            }
        }

        getRandom = Random.Range(0, totalProbabilityValue);
        for (int i = 0; i < probabilityValue.Length; i++)
        {
            if (getRandom < probabilityValue[i])
            {
                return i;
            }
            else
            {
                getRandom -= probabilityValue[i];
            }
        }
        return probabilityValue.Length - 1;

    }

    #endregion

    #region 毒圈代码

    public void LoadCirclefieldInfo()
    {
        CirclefieldInfo Info_circlefield = MeaninglessJson.LoadJsonFromFile<CirclefieldInfo>(MeaninglessJson.Path_StreamingAssets + "Circlefield.json");
        SCFs = new List<SingleCirclefield>();
        SCFs = Info_circlefield.Circlefields;
    }


    private int cur_CirclefieldNum = 0;
    private bool MoveLock = false;
    private float R = 250f;
    private Vector3 NextCircle = new Vector3();

    public void StartCirclefield()
    {
        if (SCFs.Count != 0)
        {
            if (MoveLock == false)
            {
                //圈持续倒计时
                Timer.Instance.StartCountdown(SCFs[cur_CirclefieldNum].Holdtime, () =>
                {
                    NextCircle = CalcnextCirclefieldOrigin(Circlefield.transform.position.x, Circlefield.transform.position.z, R, SCFs[cur_CirclefieldNum].ShrinkPercent);
                    R = R * SCFs[cur_CirclefieldNum].ShrinkPercent;
                    //Debug.Log("圈移动开始,下一圈坐标：" + NextCircle.ToString());

                    iTween.ScaleTo(Circlefield, iTween.Hash("x", Circlefield.transform.localScale.x * SCFs[cur_CirclefieldNum].ShrinkPercent, "z", Circlefield.transform.localScale.z * SCFs[cur_CirclefieldNum].ShrinkPercent, "time", SCFs[cur_CirclefieldNum].Movetime));
                    iTween.MoveTo(Circlefield, iTween.Hash("position", NextCircle, "time", SCFs[cur_CirclefieldNum].Movetime));

                    Timer.Instance.StartCountdown(SCFs[cur_CirclefieldNum].Movetime, () =>
                     {
                         if (cur_CirclefieldNum < SCFs.Count - 1)
                         {
                             cur_CirclefieldNum++;
                             //圈移动锁解除
                             MoveLock = false;
                             //Debug.Log("圈移动锁解除 ：" + MoveLock + " cur_CirclefieldNum:" + cur_CirclefieldNum.ToString());
                             StartCirclefield();
                         }


                     });

                });
                MoveLock = true;
            }
        }
    }

    private Vector3 CalcnextCirclefieldOrigin(float X, float Y, float Radius, float Shrinkpercent)
    {
        float distance;
        Vector2 p = new Vector2();
        for (int i = 0; i < 255; i++)
        {
            p = Random.insideUnitCircle * Radius;
            distance = Vector3.Distance(new Vector3(X, 0, Y), new Vector3(p.x, 0, p.y)) + Radius * Shrinkpercent;
            if (distance <= Radius)
            {
                break;
            }
        }
        return new Vector3(p.x, 0, p.y);
    }

    #endregion
}
