using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using Meaningless;

public class LoadingUI : BaseUI, IPointerDownHandler
{
    //AutoStatement
    private int sequence = 0;
    private Text text;
    protected override void InitUiOnAwake()
    {
        text = GameTool.GetTheChildComponent<Text>(this.gameObject, "Text");

        MessageCenter.AddListener_Multparam(EMessageType.LoadingUI,SetTips);
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.LoadingUI;
        StartCoroutine(LoadResources());
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
    IEnumerator LoadResources()
    {

        yield return new WaitForEndOfFrame();

        yield return ResourcesManager.Instance.LoadItems();
        yield return ResourcesManager.Instance.LoadMapTiles();
        ResourcesManager.Instance.LoadUITextures();

        
        GameObject mapGenerator = new GameObject("MapManager");
        mapGenerator.AddComponent<MapManager>();
        GameObject networkManager = new GameObject("NetworkManager");
        networkManager.AddComponent<NetworkManager>();

        if (ItemInfoManager.Instance.LoadInfo() == true)
        {
            sequence = 1;
            text.text = "资源加载完毕，点击进入游戏";

        }

    }

    IEnumerator InstantiateResources()
    {

        UIManager.Instance.ShowUI(UIid.HUDUI);
        UIManager.Instance.HideTheUI(UIid.LoadingUI, delegate { });


        yield return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (sequence)
        {
            case 1:
                UIManager.Instance.HideTheUI(UIid.LoadingUI, delegate { });
                UIManager.Instance.ShowUI(UIid.LoginUI);
                sequence = 2;
                break;
            case 3:

                break;
        }


    }
}
