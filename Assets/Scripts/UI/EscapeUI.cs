using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Meaningless;

public class EscapeUI : BaseUI
{
    private Button BtnBack = null;
    private Button BtnBackGame = null;
    private Button BtnQuit = null;

    protected override void InitUiOnAwake()
    {
        BtnBack = GameTool.GetTheChildComponent<Button>(this.gameObject, "BtnBack");
        BtnBack.onClick.AddListener(OnBackBtnClick);
        BtnBackGame= GameTool.GetTheChildComponent<Button>(this.gameObject, "BtnBackGame");
        BtnBackGame.onClick.AddListener(OnBtnBackGame);
        BtnQuit = GameTool.GetTheChildComponent<Button>(this.gameObject, "BtnQuit");

    }

    private void OnBackBtnClick()
    {
        NetworkManager.SendPlayerDead();
        UIManager.Instance.ShowUI(UIid.MainUI);
        UIManager.Instance.HideTheUI(UIid.EscapeUI, () => { });
    }

    private void OnBtnBackGame()
    {
        CameraBase.Instance.isFollowing = true;
        UIManager.Instance.ShowUI(UIid.HUDUI);
        UIManager.Instance.HideTheUI(UIid.EscapeUI, () => { });
    }

    private void OnBtnQuit()
    {
        NetworkManager.SendPlayerDead();
        Application.Quit();
    }

}
