﻿using UnityEngine;
using System.Collections;

namespace Meaningless
{
    //public class UIType
    //{
    //    public EShowUIMode showMode = EShowUIMode.HideOther;
    //    public EUIRootType uiRootType = EUIRootType.Normal;
    //}
    public delegate void DelAfterHideUI();
    //UI基类，封装窗体的一些共同属性以及共同的行为特征
    public class BaseUI : MonoBehaviour {
        // public UIType uiType;
        protected Transform thisTrans;
        //当前窗体的ID
        protected UIid uiId = UIid.NullUI;
        //上一个窗体的ID
        protected UIid beforUIid = UIid.NullUI;
        //显示机制
        //public EShowUIMode showMode = EShowUIMode.HideOther;
        //获取窗体的ID（只读）
        public UIid GetUiId
        {
            get
            {
                return uiId;
            }
        }
        //获取上一个窗体的ID
        public UIid GetBeforUIid
        {
            get
            {
                return beforUIid;
            }
            set
            {
                beforUIid = value;
            }
        }
        ////显示出来后是否需要处理其他窗体（1、隐藏所有 2、隐藏其他窗体，不包括最前方的 3、什么事情都不做的）
        //public bool isNeedDealWithUI
        //{
        //    get
        //    {
        //        if (this.uiType.uiRootType == EUIRootType.KeepAbove)
        //        {
        //            return false;//如果放在KeepAbove根节点下面，那么就不需要隐藏其他窗体了
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //}
        protected virtual void Awake()
        {
            thisTrans = this.transform;
            //if (uiType == null)
            //{
            //    uiType = new UIType();
            //}
            InitUiOnAwake();
            InitDataOnAwake();
        }
        protected virtual void Start()
        {
            InitInStart();
        }

        //界面元素初始化
        protected virtual void InitUiOnAwake()
        {

        }
        //初始化数据
        protected virtual void InitDataOnAwake()
        {

        }
        protected virtual void InitInStart()
        {

        }
        //窗体显示
        public virtual void ShowUI()
        {
            this.gameObject.SetActive(true);
        }
        public virtual void HideUI(DelAfterHideUI del)
        {
            this.gameObject.SetActive(false);
            if (del!=null)
            {
                del();
            }
            Save();
        }
        protected virtual void Save()
        {

        }

        //自带的脚本生命周期函数
        protected virtual void OnDestroy()
        {

        }
        //自带的脚本生命周期函数
        protected virtual void OnEnable()
        {
            PlayAudio();
        }
        //显示窗体时播放的音效
        protected virtual void PlayAudio()
        {
            
        }
    }
	
}