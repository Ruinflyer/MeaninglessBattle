using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using Meaningless;

public class LoadingUI : BaseUI
{
    //AutoStatement
    private Slider slider;
    private int progress = 0;
    protected override void InitUiOnAwake()
    {
        slider = GameTool.GetTheChildComponent<Slider>(this.gameObject, "Slider");
        slider.value = 0;
        MessageCenter.AddListener(EMessageType.Loading, Loading);
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.LoadingUI;
    }

    private void Loading(object obj)
    {
        slider.value = 0;
        switch ((int)obj)
        {
            case 0:
                progress = 0;
                StartCoroutine(LoadResources());
                break;
            case 1:
                progress = 0;
                StartCoroutine(InstantiateResources());
                break;
        }
    }

    IEnumerator LoadResources()
    {
        
        yield return new WaitForEndOfFrame();
        
        yield return ResourcesManager.Instance.LoadItems();
        yield return ResourcesManager.Instance.LoadMapTiles();

        //测试时代码
        GameObject mapGenerator = new GameObject("MapManager");
        mapGenerator.AddComponent<MapManager>();
        

        if (ItemInfoManager.Instance.LoadInfo() == true)
        {
           
            UIManager.Instance.HideTheUI(UIid.LoadingUI, delegate { });
            UIManager.Instance.ShowUI(UIid.MainUI);
        }

    }

    IEnumerator InstantiateResources()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
        

       

        if (MapManager.Instance.GenerateSquare() == true)
        {
            
            UIManager.Instance.ShowUI(UIid.HUDUI);
            UIManager.Instance.HideTheUI(UIid.LoadingUI, delegate { });
        }



        yield return true;
    }

    void SetProgress(int _progress)
    {
        slider.value = _progress * 0.01f;
    }
}
