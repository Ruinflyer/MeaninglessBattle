using UnityEngine;
using System.Collections;
using Meaningless;
using System.Collections.Generic;
//广播中心(观察者模式)
public class MessageCenter
{
    public delegate void DelCallBack(object obj);
    //一个用于存放所有监听的字典
    public static Dictionary<EMessageType, DelCallBack> dicMessageType = new Dictionary<EMessageType, DelCallBack>();
    //添加监听(传入一个广播的消息类型，监听到广播后所要执行的方法)
    public static void AddListener(EMessageType messageType, DelCallBack handler)
    {
        if (!dicMessageType.ContainsKey(messageType))
        {
            dicMessageType.Add(messageType, null);
        }
        dicMessageType[messageType] += handler;
    }
    //取消指定的监听
    public static void RemoveListener(EMessageType messageType, DelCallBack handler)
    {
        if (dicMessageType.ContainsKey(messageType))
        {
            dicMessageType[messageType] -= handler;
        }
    }
    //取消所有监听
    public static void RemoveAllListener()
    {
        dicMessageType.Clear();
    }
    //广播消息
    public static void Send(EMessageType messageType,object obj=null)
    {
        DelCallBack del;
        if (dicMessageType.TryGetValue(messageType, out del))
        {
            if (del!=null)
            {
                del(obj);
            }
        }
    }

}
