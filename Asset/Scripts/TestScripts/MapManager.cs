using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;
using MeaninglessNetwork;



public class MapManager : MonoSingleton<MapManager>
{
    //圈参数表
    private GameObject Circlefield;


    private ItemSpawnPoint ISP;
    private GameObject tmp_Maptile;
    private List<string> MapData;
    private List<int> ItemsID;
    private int X = 0;
    private int Z = 0;
    private int ItemListIndex = 0;

    // Use this for initialization
    void Start()
    {
        //LoadCirclefieldInfo();
        NetworkManager.ServerConnection.msgDistribution.AddEventListener("GetMapData", OnGetMapDataBack);
        NetworkManager.ServerConnection.msgDistribution.AddEventListener("GetMapItemData", OnGetMapItemDataBack);
        NetworkManager.ServerConnection.msgDistribution.AddEventListener("Circlefield", OnCirclefieldBack);
    }

    // Update is called once per frame
    void Update()
    {

    }

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
    private void OnGetMapItemDataBack(BaseProtocol protocol)
    {
        BytesProtocol p = (BytesProtocol)protocol;
        int startIndex = 0;
        p.GetString(startIndex, ref startIndex);
        ItemsID = new List<int>();
        int MapItemNum = p.GetInt(startIndex, ref startIndex);
        object[] param = new object[2];
        param[0] = "正在接收道具数据...";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);
        for (int i = 0; i < MapItemNum; i++)
        {
            ItemsID.Add(p.GetInt(startIndex, ref startIndex));
        }
        param[0] = "道具数据接收完毕";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);
    }
    public void GenerateMap()
    {
        if (MapData.Count > 0 && ItemsID.Count > 0)
        {
            StartCoroutine(Generate());
        }

    }

    IEnumerator Generate()
    {
        //LoadingUI提示：
        object[] param = new object[2];
        param[0] = "正在生成地图...";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);

        //生成基底平台
        Instantiate(ResourcesManager.Instance.GetMapTiles("Edge"), Vector3.zero, Quaternion.identity).transform.parent = gameObject.transform;
        //生成地图块
        for (int i = 0; i < MapData.Count; i++)
        {
            tmp_Maptile = Instantiate(ResourcesManager.Instance.GetMapTiles(MapData[i]), new Vector3(X, 0, Z), Quaternion.identity);
            tmp_Maptile.transform.parent = gameObject.transform;
            //地上物品生成：
            ISP = tmp_Maptile.GetComponent<ItemSpawnPoint>();
            if (ISP != null)
            {
                for (int j = 0; j < ISP.ItemSpawnPoints.Length; j++)
                {
                    Instantiate(ResourcesManager.Instance.GetItem(ItemInfoManager.Instance.GetItemName(ItemsID[ItemListIndex])),
                        new Vector3(ISP.ItemSpawnPoints[j].position.x, 0, ISP.ItemSpawnPoints[j].position.z), Quaternion.identity);

                    if (ItemListIndex == 127)
                    {
                        ItemListIndex = 0;
                    }
                    else
                    {
                        ItemListIndex++;
                    }

                }

            }

            X += 50;
            if (i % 7 == 0)
            {
                Z += 50;
                X = 0;
            }

        }

        param[0] = "地图生成完毕";
        param[1] = 2;
        MessageCenter.Send_Multparam(EMessageType.LoadingUI, param);

        yield return true;
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
