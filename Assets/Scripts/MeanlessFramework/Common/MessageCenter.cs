using UnityEngine;
using System.Collections;
using Meaningless;
using System.Collections.Generic;

/// <summary>
/// 广播中心
/// </summary>
public class MessageCenter
{
    public delegate void DelCallBack(object obj);
    public delegate void MultDelCallBack(object[] objs);
    //存放单参数监听的字典
    public static Dictionary<EMessageType, DelCallBack> dicMessageType = new Dictionary<EMessageType, DelCallBack>();
    //存放多参数监听的字典
    public static Dictionary<EMessageType, MultDelCallBack> dicMultMessageType = new Dictionary<EMessageType, MultDelCallBack>();

    /// <summary>
    /// 单参数-添加监听
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="handler"></param>
    public static void AddListener(EMessageType messageType, DelCallBack handler)
    {
        if (!dicMessageType.ContainsKey(messageType))
        {
            dicMessageType.Add(messageType, null);
        }
        dicMessageType[messageType] += handler;
    }
    /// <summary>
    /// 多参数-添加监听
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="handler"></param>
    public static void AddListener_Multparam(EMessageType messageType, MultDelCallBack handler)
    {
        if (!dicMultMessageType.ContainsKey(messageType))
        {
            dicMultMessageType.Add(messageType, null);
        }
        dicMultMessageType[messageType] += handler;
    }
    /// <summary>
    ///  单参数-取消监听
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="handler"></param>
    public static void RemoveListener(EMessageType messageType, DelCallBack handler)
    {
        if (dicMessageType.ContainsKey(messageType))
        {
            dicMessageType[messageType] -= handler;
        }
    }
    /// <summary>
    /// 多参数-取消监听
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="handler"></param>
    public static void RemoveListener_Multparam(EMessageType messageType, MultDelCallBack handler)
    {
        if (dicMultMessageType.ContainsKey(messageType))
        {
            dicMultMessageType[messageType] -= handler;
        }
    }
    /// <summary>
    /// 取消所有监听
    /// </summary>
    public static void RemoveAllListener()
    {
        dicMessageType.Clear();
        dicMultMessageType.Clear();
    }

    /// <summary>
    /// 单参数-消息广播
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="obj"></param>
    public static void Send(EMessageType messageType, object obj = null)
    {
        DelCallBack del;
        if (dicMessageType.TryGetValue(messageType, out del))
        {
            if (del != null)
            {
                del(obj);
            }
        }
    }
    /// <summary>
    /// 多参数-消息广播
    /// </summary>
    /// <param name="messageType"></param>
    /// <param name="objs"></param>
    public static void Send_Multparam(EMessageType messageType, object[] objs = null)
    {
        MultDelCallBack del;
        if (dicMultMessageType.TryGetValue(messageType, out del))
        {
            if (del != null)
            {
                del(objs);
            }
        }
    }
}
