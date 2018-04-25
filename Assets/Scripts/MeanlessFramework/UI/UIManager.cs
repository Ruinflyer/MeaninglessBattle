using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Meaningless
{
    public class UIManager : MonoSingleton<UIManager>
    {
        //缓存所有打开过的窗体
        private Dictionary<UIid, BaseUI> dicAllUI;
        //缓存所有的正在显示的窗体
        private Dictionary<UIid, BaseUI> dicShowUI;

        //缓存最近显示出来的界面
        private BaseUI currentUI = null;
        //缓存切换过来的上一个窗体
        private BaseUI beforeUI = null;
        //新的窗体显示出来之后，缓存被隐藏的窗体的ID
        private UIid beforeHidUIid = UIid.NullUI;
        //缓存画布
        private Transform canvas;
        //所有窗体的父节点
        private Transform uiRoot;
        
        private void Awake()
        {
            canvas = this.transform.parent;
            if (dicAllUI==null)
            {
                dicAllUI = new Dictionary<UIid, BaseUI>();
            }
            if (dicShowUI==null)
            {
                dicShowUI = new Dictionary<UIid, BaseUI>();
            }
            //初始化UIManager
            InitUIManager();
        }
        private void InitUIManager()
        {
            if (dicAllUI!=null)
            {
                dicAllUI.Clear();
            }
            if (dicShowUI!=null)
            {
                dicShowUI.Clear();
            }
            uiRoot = GameObject.Find("UIRoot").transform;
            //切换场景的时候，不让Canvas被销毁
            DontDestroyOnLoad(canvas);
            
            //显示主窗体
            ShowUI(UIid.HUDUI);
           
        }
        //显示窗体的方法(isSaveBeforUIid是否要存储上一个跳转过来的窗体ID)
        public void ShowUI(UIid uiId,bool isSaveBeforUIid=true)
        {
           
            if (uiId==UIid.NullUI)
            {
                uiId = UIid.MainUI;
            }
            BaseUI baseUI = JudgeShowUI(uiId);
            if (baseUI!=null)
            {
                baseUI.ShowUI();
                if (isSaveBeforUIid)
                {
                    baseUI.GetBeforUIid = beforeHidUIid;
                }
            }

        }
        public void ReturnUI(UIid beforUIid)
        {
            ShowUI(beforUIid,false);
        }
        //判断窗体是否有显示过隐藏起来了，如果没有，就要去加载了
        private BaseUI JudgeShowUI(UIid uiId)
        {
            //判断要显示的窗体是不是正在显示
            if (dicShowUI.ContainsKey(uiId))
            {
                return null;
            }
            //判断窗体是不是显示过了，然后被隐藏起来了
            BaseUI baseUI = GetBaseUI(uiId);
            if (baseUI==null)//说明将要显示的窗体还未被加载过
            {
                if (GameDefine.dicPath.ContainsKey(uiId))//有该窗体的加载路径
                {
                    string path = GameDefine.dicPath[uiId];
                    GameObject theUI = Resources.Load<GameObject>(path);
                    if (theUI!=null)
                    {
                        //把窗体生成出来
                        GameObject willShowUI = Instantiate(theUI);
                        //判断显示的窗体上面是否有挂载UI脚本
                        baseUI = willShowUI.GetComponent<BaseUI>();
                        if (baseUI==null)
                        {
                            //自动挂载对应的UI脚本
                            Type type = GameDefine.GetUIScriptType(uiId);
                            baseUI = willShowUI.AddComponent(type)as BaseUI;
                        }
                        GameTool.AddChildToParent(uiRoot, willShowUI.transform);
                        willShowUI.GetComponent<RectTransform>().sizeDelta=Vector2.zero;
                        willShowUI.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                        dicAllUI.Add(uiId,baseUI);

                    }
                    else
                    {
                        Debug.LogError("在路径"+ path + "下面加载不到窗体，请查看该路径下面是否有窗体的预制体");
                    }
                }
                else//没有该窗体的加载路径
                {
                    Debug.LogError("没有该窗体的路径，请到GameDefine里面去添加");
                }
            }
            //更新字典并且隐藏需要隐藏的UI
            UpdateDicAndHideUI(baseUI);
            return baseUI;
        }
        //更新字典并且隐藏需要隐藏的UI
        private void UpdateDicAndHideUI(BaseUI baseUI)
        {

                //存储被隐藏的界面
               // List<UIid> listRemove = new List<UIid>();
                if (dicShowUI.Count>0)
                {
                    
                        foreach (KeyValuePair<UIid,BaseUI> UiItem in dicShowUI)
                        {
                            
                                    //隐藏其他UI
                                    UiItem.Value.HideUI(null);
                                    //缓存上一个窗体的ID
                                    beforeHidUIid = UiItem.Key;
                                    //listRemove.Add(beforeHidUIid);

                         
                           
                        }
                  dicShowUI.Clear();
                }
                //if (listRemove!=null)
                //{
                //    for (int i = 0; i < listRemove.Count; i++)
                //    {
                //        dicShowUI.Remove(listRemove[i]);
                //    }
                //}
          
            //把显示出来的窗体添加进dicShowUI这个字典
            dicShowUI.Add(baseUI.GetUiId,baseUI);
        }
        //隐藏单个窗体
        public void HideTheUI(UIid UiId, DelAfterHideUI del)
        {
            if (!dicShowUI.ContainsKey(UiId))
            {
                return;
            }

            if (del!=null)
            {
                //执行委托里面的方法
                dicShowUI[UiId].HideUI(del);
               // dicShowUI.Remove(UiId);
            }
            else
            {
                dicShowUI[UiId].HideUI(null);
                //dicShowUI.Remove(UiId);
            }
            dicShowUI.Remove(UiId);

        }
        //隐藏所有窗体
        public void HideAllUI(bool isNeedHideAboveUI)
        {
            if (isNeedHideAboveUI)
            {   //隐藏掉所有正在显示的窗体（包括前方窗体keepAvove）
                foreach (KeyValuePair<UIid, BaseUI> uiItem in dicShowUI)
                {
                    uiItem.Value.HideUI(null);
                }
                dicShowUI.Clear();
            }
            else
            {
                //隐藏掉所有窗体，但是，不包括keepAbove的窗体
                List<UIid> listRemove = new List<UIid>();
                foreach (KeyValuePair<UIid, BaseUI> uiItem in dicShowUI)
                {
                        //隐藏起来
                        uiItem.Value.HideUI(null);
                        listRemove.Add(uiItem.Key);
                }
                for (int i = 0; i < listRemove.Count; i++)
                {
                    dicShowUI.Remove(listRemove[i]);
                }
            }
        }

        private BaseUI GetBaseUI(UIid uiId)
        {
            if (dicAllUI.ContainsKey(uiId))
            {
                return dicAllUI[uiId];
            }
            else
            {
                return null;
            }
    
        }
       
    }
}
