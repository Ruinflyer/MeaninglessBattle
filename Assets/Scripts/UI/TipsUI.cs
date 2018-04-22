using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Meaningless;
public class TipsUI : BaseUI
{
    //AutoStatement
    private Button Btn_OK = null;
    private Text Text_Tips = null;
    private Text Text_Button = null;

    protected override void InitUiOnAwake()
    {
        Btn_OK = GameTool.GetTheChildComponent<Button>(this.gameObject, "Btn_OK");
        Btn_OK.onClick.AddListener(OK);

        Text_Button = GameTool.GetTheChildComponent<Text>(GameTool.FindTheChild(this.gameObject, "Btn_OK").gameObject, "Text");
        Text_Tips = GameTool.GetTheChildComponent<Text>(this.gameObject, "Text_Tips");

        MessageCenter.AddListener_Multparam(EMessageType.TipsUI, SetTips);

    }
    protected override void InitDataOnAwake()
    {
        this.uiId = UIid.TipsUI;
    }

    protected void SetTips(object[] objs)
    {
        if(objs!=null)
        {
            Text_Tips.text = (string)objs[0];
            Text_Button.text = (string)objs[1];
        }
        
    }

    protected void OK()
    {
        UIManager.Instance.HideTheUI(this.uiId,()=>{ });
        UIManager.Instance.ReturnUI(this.beforeUIid);
    }
}
