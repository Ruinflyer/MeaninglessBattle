using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class NetworkCenter : Photon.PunBehaviour
{
    
    /*实例参数声明区*/
    protected static NetworkCenter _instance;
    public static NetworkCenter Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find(typeof(NetworkCenter).Name);
                if (go == null)
                {
                    go = new GameObject(typeof(NetworkCenter).Name);
                    GameObject.DontDestroyOnLoad(go);
                    

                }
                _instance = go.AddComponent<NetworkCenter>();
                GlobalPhotonview = go.AddComponent<PhotonView>();
                GlobalPhotonview.viewID = 1;
                Debug.Log("网络中心 NetworkCenter 创建成功");
            }
            return _instance;
        }
    }

    /*变量声明区*/
    //全局PhotonView
    protected static PhotonView GlobalPhotonview;
    //房间属性
    private RoomOptions roomOptions;

    
    public PhotonView GetGlobalPhotonview()
    {
        return GlobalPhotonview;
    }


    /// <summary>
    /// 加入房间时
    /// </summary>
    public override void OnJoinedRoom()
    {
        //同步加入人数
        MessageCenter.Send(Meaningless.EMessageType.JoinedPlayers, PhotonNetwork.playerList.Length);
        GlobalPhotonview.RPC("UpdatePlayersCount", PhotonTargets.Others);
        if(!PhotonNetwork.isMasterClient)
        {
            GameObject man = PhotonNetwork.Instantiate("TestMan", new Vector3(-55, 51, -55), Quaternion.identity, 0);
            GameObject.FindWithTag("MainCamera").GetComponent<TestCam>().FindPlayer();
        }
        

    }

    /// <summary>
    /// 创建房间成功时
    /// </summary>
    public override void OnCreatedRoom()
    {
        MessageCenter.Send(Meaningless.EMessageType.JoinedPlayers, PhotonNetwork.playerList.Length);
        GameObject man = PhotonNetwork.Instantiate("TestMan", new Vector3(-55, 51, -55), Quaternion.identity, 0);
        GameObject.FindWithTag("MainCamera").GetComponent<TestCam>().FindPlayer();
    }

    /// <summary>
    /// 连接到主服务器时
    /// </summary>
    public override void OnConnectedToMaster()
    {

        roomOptions = new RoomOptions()
        {
            IsOpen = true,
            MaxPlayers = 2,
            PublishUserId = true
        };
        PhotonNetwork.JoinOrCreateRoom("ROOM", roomOptions, null);

    }

    /// <summary>
    /// 玩家连接时
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if(PhotonNetwork.playerList.Length==roomOptions.MaxPlayers)
        {
            MapManager.Instance.GenerateMap();
        }


    }


    /// <summary>
    /// 刷新玩家数量
    /// </summary>
    [PunRPC]
    public void UpdatePlayersCount()
    {
        MessageCenter.Send(Meaningless.EMessageType.JoinedPlayers, PhotonNetwork.playerList.Length);
    }


    GameObject tmp_Maptile;

    /// <summary>
    /// 生成地图块
    /// </summary>
    /// <param name="MapTileName">地块名</param>
    /// <param name="pos">坐标</param>
    [PunRPC]
    void GenerateMapTileOnNetwork(string MapTileName, Vector3 pos)
    {
        tmp_Maptile = Instantiate(ResourcesManager.Instance.GetMapTiles(MapTileName), pos, Quaternion.identity);
    }

    /// <summary>
    /// 生成物件
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="rot">旋转度</param>
    /// <param name="itemName">物品名称</param>
    /// <param name="photonViewID">同步viewID</param>
    [PunRPC]
    void SpawnItem(Vector3 pos, Quaternion rot, string itemName, int photonViewID)
    {
        GameObject obj = Instantiate(ResourcesManager.Instance.GetItem(itemName), pos, rot);
        obj.GetComponent<PhotonView>().viewID = photonViewID;
    }

    /// <summary>
    /// 同步门ID
    /// </summary>
    /// <param name="index">数组下标</param>
    /// <param name="photonViewID">viewID</param>
    [PunRPC]
    void SyncDoorID(int index, int photonViewID)
    {
        if (tmp_Maptile != null)
        {
         tmp_Maptile.GetComponent<ItemSpawnPoint>().DoorSpawnPoints[index].gameObject.GetComponent<PhotonView>().viewID = photonViewID;
        }
    }

   


}
