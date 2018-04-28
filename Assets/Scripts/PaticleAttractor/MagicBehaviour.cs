using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meaningless;

public class MagicBehaviour : MonoBehaviour {

    public MagicType magicType;
    public PlayerController player;
    private 

	// Use this for initialization
	void Start () {
        player = CameraBase.Instance.player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		switch(magicType)
        {
            case MagicType.Ripple:
                Damage(1, 30);
                break;
            case MagicType.HeartAttack:

                break;
            case MagicType.IceArrow:
                float IProbability = ItemInfoManager.Instance.GetItemInfo(603).magicProperties.Probability;
                    break;
            case MagicType.ChoshimArrow:
                float CProbability = ItemInfoManager.Instance.GetItemInfo(604).magicProperties.Probability;
                break;
            case MagicType.StygianDesolator:
                break;
            case MagicType.Thunderbolt:
                break;
            case MagicType.KillerQueen:
                break;
        }

	}

    void Damage(float distance,float angle)
    {
        foreach (KeyValuePair<string, NetworkPlayer> enemy in player.ScenePlayers)
        {
            if (player.CheckCanAttack(gameObject, enemy.Value.gameObject, distance, angle))
            {
                NetworkManager.SendPlayerHitSomeone(enemy.Value.name, player.characterStatus.Attack_Magic * (1 - enemy.Value.status.Defend_Magic / 100));
            }
        }
    }

    void Freeze(float buffTime, float distance, float angle)
    {
        foreach (KeyValuePair<string, NetworkPlayer> enemy in player.ScenePlayers)
        {
            if (player.CheckCanAttack(gameObject, enemy.Value.gameObject, distance, angle))
            {
                enemy.Value.playerController.GetDeBuffInTime(BuffType.Freeze, buffTime);

            }
        }
    }

    void Blind(float buffTime,float distance, float angle)
    {
        foreach (KeyValuePair<string, NetworkPlayer> enemy in player.ScenePlayers)
        {
            if (player.CheckCanAttack(gameObject, enemy.Value.gameObject, distance, angle))
            {
                //本地效果
                enemy.Value.playerController.GetDeBuffInTime(BuffType.Blind, buffTime);
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
                enemy.Value.playerController.GetDeBuffInTime(BuffType.SlowDown, buffTime);
            }
        }
    }
}
