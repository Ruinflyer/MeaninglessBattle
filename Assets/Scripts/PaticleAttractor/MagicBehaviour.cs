﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class MagicBehaviour : MonoBehaviour
{

    public MagicType magicType;
    public PlayerController player;
    public bool isHit = false;

    // Use this for initialization
    void Start()
    {
        player = CameraBase.Instance.player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            switch (magicType)
            {
                case MagicType.Ripple:
                    Damage(1, 30);
                    break;
                case MagicType.HeartAttack:
                    Damage(1, 30);
                    break;
                case MagicType.IceArrow:
                    float IProbability = ItemInfoManager.Instance.GetItemInfo(603).magicProperties.Probability;
                    Damage(1, 30);
                    if (Random.value < IProbability)
                    {
                        Freeze(2, 1, 30);
                    }
                    break;
                case MagicType.ChoshimArrow:
                    float CProbability = ItemInfoManager.Instance.GetItemInfo(604).magicProperties.Probability;
                    Damage(1, 30);
                    if (Random.value < CProbability)
                    {
                        SlowDown(4, 1, 30);
                    }
                    break;
                case MagicType.StygianDesolator:
                    Blind(5, 5, 360);
                    break;
                case MagicType.Thunderbolt:
                    Damage(5, 360);
                    break;
            }
            isHit = false;
        }

    }

    void Damage(float distance, float angle)
    {
        foreach (KeyValuePair<string, NetworkPlayer> enemy in player.ScenePlayers)
        {
            if (player.CheckCanAttack(gameObject, enemy.Value.gameObject, distance, angle))
            {
                NetworkManager.SendPlayerHitSomeone(enemy.Value.name, BagManager.Instance.GetCharacterStatus().Attack_Magic * (1 - enemy.Value.status.Defend_Magic / 100));
            }
        }
    }

    void Freeze(float buffTime, float distance, float angle)
    {
        foreach (KeyValuePair<string, NetworkPlayer> enemy in player.ScenePlayers)
        {
            if (player.CheckCanAttack(gameObject, enemy.Value.gameObject, distance, angle))
            {
                enemy.Value.playerController.GetDeBuffInTime(BuffType.Freeze, buffTime, enemy.Value.status);

            }
        }
    }

    void Blind(float buffTime, float distance, float angle)
    {
        foreach (KeyValuePair<string, NetworkPlayer> enemy in player.ScenePlayers)
        {
            if (player.CheckCanAttack(gameObject, enemy.Value.gameObject, distance, angle))
            {
                //本地效果
                enemy.Value.playerController.GetDeBuffInTime(BuffType.Blind, buffTime, enemy.Value.status);
            }
        }
    }

    void SlowDown(float buffTime, float distance, float angle)
    {
        foreach (KeyValuePair<string, NetworkPlayer> enemy in player.ScenePlayers)
        {
            if (player.CheckCanAttack(gameObject, enemy.Value.gameObject, distance, angle))
            {
                //本地效果
                enemy.Value.playerController.GetDeBuffInTime(BuffType.SlowDown, buffTime, enemy.Value.status);
            }
        }
    }
}
