using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using Meaningless;
using MeaninglessNetwork;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadingUI : BaseUI, IPointerDownHandler
{
    //AutoStatement
    private int sequence = 0;
    //加载序列,为0时加载
    private Text text;


    protected override void InitUiOnAwake()
    {
        text = GameTool.GetTheChildComponent<Text>(this.gameObject, "Text");

        MessageCenter.AddListener_Multparam(EMessageType.LoadingUI, SetTips);
        MessageCenter.AddListener(EMessageType.LoadingScene, LoadScene);
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.LoadingUI;
        Loading(0);
    }


    private void Loading(int LoadingSequence)
    {
        switch (LoadingSequence)
        {
            case 0:
                StartCoroutine(LoadResources());
                break;
            case 1:
                StartCoroutine(InstantiateResources());
                break;
        }
    }

    protected void SetTips(object[] objs)
    {
        if (objs != null)
        {
            text.text = (string)objs[0];
            sequence = (int)objs[1];
        }
    }

    protected void LoadScene(object obj)
    {
        Loading(1);
    }
    IEnumerator LoadResources()
    {

        yield return new WaitForEndOfFrame();

        yield return ResourcesManager.Instance.LoadItems();
        //yield return ResourcesManager.Instance.LoadMapTiles();
        ResourcesManager.Instance.LoadUITextures();
       
        ResourcesManager.Instance.LoadSceneAndGetSceneName();

        //网络管理器
        GameObject networkManager = new GameObject("NetworkManager");
        networkManager.AddComponent<NetworkManager>();
        DontDestroyOnLoad(networkManager);

        if (ItemInfoManager.Instance.LoadInfo() == true)
        {
            sequence = 1;
            text.text = "资源加载完毕，点击进入游戏";
        }

        

    }

    IEnumerator InstantiateResources()
    {

        AsyncOperation async = SceneManager.LoadSceneAsync(ResourcesManager.Instance.sceneName);
        yield return async;

        
        //SceneManager.LoadScene(ResourcesManager.Instance.sceneName);
        MapManager mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        //网络玩家管理器
        //NetworkPlayerManager networkPlayerManager = GameObject.Find("NetworkPlayerManager").AddComponent<NetworkPlayerManager>();

        //请求地图数据
        mapManager.RequestItemData();
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        switch (sequence)
        {
            case 1:
                UIManager.Instance.HideTheUI(UIid.LoadingUI, delegate { });
                UIManager.Instance.ShowUI(UIid.LoginUI);
                sequence = 0;
                break;
            case 2:
                UIManager.Instance.HideTheUI(UIid.LoginUI, delegate { });
                UIManager.Instance.HideTheUI(UIid.MainUI, delegate { });
                UIManager.Instance.HideTheUI(UIid.RoomUI, delegate { });
                UIManager.Instance.HideTheUI(UIid.LoadingUI, delegate { });
                UIManager.Instance.ShowUI(UIid.HUDUI);
                break;
        }


    }

}
