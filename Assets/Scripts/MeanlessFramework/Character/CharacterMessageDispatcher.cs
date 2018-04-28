using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class CharacterMessage
{
    //处理消息的实体
    public BaseFSM Receiver;
    //状态切换的枚举
    public FSMTransitionType Msg;
    //消息附带的额外信息，这个可以根据需求自定义，也可以为null
    public bool condition;

    public CharacterMessage(FSMTransitionType msg, BaseFSM rid,bool con)
    {
        Msg = msg;
        Receiver = rid;
        condition = con;
    }
}


public class CharacterMessageDispatcher : Singleton<CharacterMessageDispatcher>
{

    public void DispatchMesssage(FSMTransitionType msg, BaseFSM receiverId, bool condition)
    {
        CharacterMessage message = new CharacterMessage(msg, receiverId, condition);
        if (message.condition)
        {
            message.Receiver.PerformTransition(message.Msg);
        }

    }
}
