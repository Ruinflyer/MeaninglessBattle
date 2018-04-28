using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeaninglessNetwork;

//协议委托
public delegate void DelegateEvent(BaseProtocol protocol);

//消息分发类
public class MsgDistribution
{
    //每帧处理消息数量
    public int HandleNum = 15;
    //消息列表
    public List<BaseProtocol> msgList = new List<BaseProtocol>();
   
    //事件监听表：
    //监听事件：注册后永久可执行
    private Dictionary<string, DelegateEvent> Dict_Event = new Dictionary<string, DelegateEvent>();
    //单次监听事件：执行方法后马上删除事件
    private Dictionary<string, DelegateEvent> Dict_Once = new Dictionary<string, DelegateEvent>();

    public void Update()
    {
        for (int i = 0; i < HandleNum; i++)
        {
            if (msgList.Count > 0)
            {
                DispatchMsgEvent(msgList[0]);
                lock (msgList)
                {
                    msgList.RemoveAt(0);
                }
                    
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 执行消息列表中的消息
    /// </summary>
    /// <param name="protocol"></param>
    public void DispatchMsgEvent(BaseProtocol protocol)
    {
        string protocolName = protocol.GetProtocolName();
        if (Dict_Event.ContainsKey(protocolName))
        {
            Dict_Event[protocolName](protocol);
        }
        if (Dict_Once.ContainsKey(protocolName))
        {
            Dict_Once[protocolName](protocol);
            Dict_Once[protocolName] = null;
            Dict_Once.Remove(protocolName);
        }
    }

    public void AddEventListener(string MethodName, DelegateEvent Callback)
    {
        if (Dict_Event.ContainsKey(MethodName))
        {
            Dict_Event[MethodName] += Callback;
        }
        else
        {
            Dict_Event[MethodName] = Callback;
        }
    }

    public void AddOnceEventListener(string MethodName, DelegateEvent Callback)
    {
        if (Dict_Once.ContainsKey(MethodName))
        {
            Dict_Once[MethodName] += Callback;
        }
        else
        {
            Dict_Once[MethodName] = Callback;
        }
    }

    public void DeleteEventListener(string MethodName, DelegateEvent Callback)
    {
        if (Dict_Event.ContainsKey(MethodName))
        {
            Dict_Event[MethodName] -= Callback;
            if (Dict_Event[MethodName] == null)
            {
                Dict_Event.Remove(MethodName);
            }
        }
    }
    public void DeleteOnceEventListener(string MethodName, DelegateEvent Callback)
    {
        if (Dict_Once.ContainsKey(MethodName))
        {
            Dict_Once[MethodName] -= Callback;
            if (Dict_Once[MethodName] == null)
            {
                Dict_Once.Remove(MethodName);
            }
        }
    }
}
