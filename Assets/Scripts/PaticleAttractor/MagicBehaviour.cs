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
<<<<<<< HEAD
         
                NetworkManager.SendPlayerHitSomeone(enemy.Value.name, player.characterStatus.Attack_Magic * (1 - enemy.Value.status.Defend_Magic / 100));


=======
                NetworkManager.SendPlayerHitSomeone(enemy.name, player.characterStatus.Attack_Magic * (1 - enemy.status.Defend_Magic / 100));
                //单机测试
                //enemy.playerFSM.characterStatus.HP -= player.characterStatus.Attack_Magic* (1 - enemy.playerFSM.characterStatus.Defend_Magic / 100);
>>>>>>> 6f47cbe27dec7f2371ed3ce6561216c863c37b7b
            }
        }
    }

    void Freeze(float buffTime, float distance, float angle)
    {
        foreach (KeyValuePair<string, NetworkPlayer> enemy in player.ScenePlayers)
        {
            if (player.CheckCanAttack(gameObject, enemy.Value.gameObject, distance, angle))
            {
<<<<<<< HEAD
                //本地效果
                enemy.Value.playerController.GetDeBuffInTime(BuffType.Freeze, buffTime);
=======

                //单机测试
                enemy.playerController.GetDeBuffInTime(BuffType.Freeze, buffTime);
>>>>>>> 6f47cbe27dec7f2371ed3ce6561216c863c37b7b
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
