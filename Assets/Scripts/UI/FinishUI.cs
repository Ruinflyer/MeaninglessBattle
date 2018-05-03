using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Meaningless;
public class FinishUI : BaseUI
{
    //AutoStatement
    private Button BtnBack = null;
    private Text Title = null;
    private Text Rank = null;
    private Text Result = null;

    protected override void InitUiOnAwake()
    {
        BtnBack = GameTool.GetTheChildComponent<Button>(this.gameObject, "BtnBack");
        BtnBack.onClick.AddListener(OnBackBtnClick);
        Title = GameTool.GetTheChildComponent<Text>(gameObject, "Title");
        Rank = GameTool.GetTheChildComponent<Text>(gameObject, "Rank");
        Result = GameTool.GetTheChildComponent<Text>(gameObject, "Result");

        MessageCenter.AddListener_Multparam(EMessageType.FinishUI, SetData);
    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.FinishUI;
    }

    private void OnBackBtnClick()
    {
        UIManager.Instance.ShowUI(UIid.MainUI);
        UIManager.Instance.HideTheUI(UIid.FinishUI,() => { });
    }

    private void SetData(object[] objs)
    {
        Title.text = objs[0] as string;
        Result.text = objs[1] as string;
        Rank.text = objs[2] as string;
    }

}
