using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using System;
using MeaninglessNetwork;

public class Connect
{
    //缓冲区长度
    public const int BUFFER_SIZE = 1024;
    public Socket socket;

    public byte[] buff=new byte[BUFFER_SIZE];
    public int buffCount = 0;

    //包头-32位无符号整数保存消息长度
    public byte[] lengthBytes = new byte[sizeof(Int32)];
    public Int32 msgLength = 0;
    //当前协议
    public BaseProtocol protocol;
    //心跳时间
    public float lastTick = 0;
    public float HeartBeatTime = 150;
    //消息分发
    public MsgDistribution msgDistribution = new MsgDistribution();

    public enum ConnectionStatus
    {
        Null,
        Connected
    };
    public ConnectionStatus connectionStatus = ConnectionStatus.Null;

    public bool ConnectToServer(string host, int port)
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(host, port);
            socket.BeginReceive(buff, buffCount, BUFFER_SIZE - buffCount, SocketFlags.None, ReceiveCallBack, buff);
            Debug.Log("连接服务器成功");
            connectionStatus = ConnectionStatus.Connected;
            return true;
        }
        catch (Exception e)
        {
            Debug.Log("连接服务器失败：" + e.Message);
            return false;
        }
    }
    private void ReceiveCallBack(IAsyncResult ar)
    {

        try
        {
            //结束异步接收
            int count = socket.EndReceive(ar);
            buffCount += count;
            PacketProcess();

            socket.BeginReceive(buff, buffCount, BUFFER_SIZE - buffCount, SocketFlags.None, ReceiveCallBack, buff);
        }
        catch (Exception e)
        {
            Debug.LogError("调用异步接收失败：" + e.Message);
            connectionStatus = ConnectionStatus.Null;
        }


    }
    private void PacketProcess()
    {
        if (buffCount < sizeof(Int32))
        {
            return;
        }
        Array.Copy(buff, lengthBytes, sizeof(Int32));
        msgLength = BitConverter.ToInt32(lengthBytes, 0);
        if (buffCount < msgLength + sizeof(Int32))
        {
            return;
        }
        BaseProtocol p = protocol.Decode(buff, sizeof(Int32), msgLength);
        //Debug.Log("收到消息：" + p.GetDescription());
        //多线程-处理需为msgList加锁
        lock (msgDistribution)
        {
            msgDistribution.msgList.Add(p);
        }

        //删除已处理的消息
        int count = buffCount - msgLength - sizeof(Int32);//缓冲区长度-当前消息体长度-消息头长度
        Array.Copy(buff, sizeof(Int32) + msgLength, buff, 0, count);//缓冲区字节数组删除已处理的消息
        buffCount = count;
        if (buffCount > 0)
        {
            PacketProcess();
        }
    }
    public bool Send(BaseProtocol protocol)
    {
        if (connectionStatus != ConnectionStatus.Connected)
        {
            Debug.LogError("发送消息失败，并未连接到服务器");
            return false;
        }
        byte[] encode = protocol.Encode();
        byte[] length = BitConverter.GetBytes(encode.Length);
        byte[] sendbyte = length.Concat(encode).ToArray();
        socket.Send(sendbyte);
        //Debug.Log("发送消息：" + protocol.GetDescription());
        return true;
    }

    /// <summary>
    /// 发送消息并监听服务端的返回值
    /// </summary>
    /// <param name="protocol">发送的协议</param>
    /// <param name="callbackName">返回的协议名callbackName时即调用委托事件</param>
    /// <param name="delegateEvent">返回后调用的事件</param>
    /// <returns></returns>
    public bool Send(BaseProtocol protocol, string callbackName, DelegateEvent delegateEvent)
    {
        if (connectionStatus != ConnectionStatus.Connected)
        {
            Debug.LogError("发送消息失败，并未连接到服务器");
            return false;
        }

        msgDistribution.AddOnceEventListener(callbackName, delegateEvent);

        return Send(protocol);
    }
    /// <summary>
    /// 发送消息并监听服务端的返回值
    /// </summary>
    /// <param name="protocol">发送的协议</param>
    /// <param name="delegateEvent">返回后调用的事件</param>
    /// <returns></returns>
    public bool Send(BaseProtocol protocol, DelegateEvent delegateEvent)
    {
        return Send(protocol, protocol.GetProtocolName(), delegateEvent);
    }
    public bool Close()
    {
        try
        {
            socket.Close();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("关闭连接失败：" + e.Message);
            return false;
        }
    }

    public void Update()
    {
        msgDistribution.Update();
        if (connectionStatus == ConnectionStatus.Connected)
        {
            if (Time.time - lastTick > HeartBeatTime)
            {
                BaseProtocol protocol = NetworkManager.GetHeartBeat();
                Send(protocol);
                lastTick = Time.time;
            }
        }
    }

}
