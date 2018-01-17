using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Meaningless
{
    //窗体的ID
    public enum UIid
    {
        NullUI = 0
		/*UIIDHere*/

    }
    public enum EMessageType
    {
        NullType,
        
    }

    public class GameDefine : MonoBehaviour
    {
        public static Dictionary<UIid, string> dicPath = new Dictionary<UIid, string>()
        {
			/*UIDictHere*/
                       

        };
        public static Type GetUIScriptType(UIid uiId)
        {
            Type scriptType = null;
            switch (uiId)
            {
                /*case UIid.MainUI:
                    scriptType = typeof(MainUI);
                    break;
                 */
				/*SwitchUIScriptType*/
                case UIid.NullUI:
                    Debug.LogError("传入的窗体ID为NullUI");
                    break;
                default:
                    Debug.LogError("没有添加对应的case条件，找不到对应的UI脚本");
                    break;
            }
            return scriptType;
        }
    }
}
