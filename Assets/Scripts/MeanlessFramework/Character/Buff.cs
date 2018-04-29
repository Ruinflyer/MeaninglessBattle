using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;



public class Buff
{
    public bool canUpdate;
    public bool canDestory;
    public BuffType type;
    CharacterStatus status;
    float buffTime;

    float keepTime;

    public delegate void EnterDel(BuffType type,CharacterStatus status);
    public delegate void ExitDel(BuffType type, CharacterStatus status);

    private EnterDel enterDel;
    private ExitDel exitDel;

    public Buff(BuffType type, float buffTime, CharacterStatus status,EnterDel enterDel, ExitDel exitDel)
    {
        this.type = type;
        this.buffTime = buffTime;
        this.enterDel = enterDel;
        this.exitDel = exitDel;
        this.status = status;
    }

    public void OnEnter()
    {
        canUpdate = true;
        if (enterDel != null)
            enterDel(this.type, status);
    }

    public void OnUpdate()
    {
        if (!canUpdate)
            return;
        keepTime += Time.deltaTime;
        if (keepTime >= buffTime)
        {
            keepTime = 0;
            canDestory = true;
        }
    }
    public void OnExit()
    {
        if (!canDestory)
            return;
        if (exitDel!=null)
            exitDel(type, status);
    }
}