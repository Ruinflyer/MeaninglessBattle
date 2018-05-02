using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meaningless
{

    public class AnimationManager : MonoBehaviour
    {

        public AnimatorStateInfo baseStateInfo;
        public AnimatorStateInfo attackStateInfo;
        public Animator anim;
        public string animCur;
        public int LayerCur;
        public bool AnimLock=false;
        public string AnimStartName;
        void Start()
        {
            anim = GetComponent<Animator>();
            
        }

        private void Update()
        {
            baseStateInfo = anim.GetCurrentAnimatorStateInfo(0);
            attackStateInfo= anim.GetCurrentAnimatorStateInfo(1);
        }

        public void PlayAnimation(string playAnim)
        {
            anim.SetBool(animCur, false);
            anim.SetBool(playAnim, true);
            animCur = playAnim;
        }

        public void PlayAnimation(string playAnim,int value)
        {
            anim.SetBool(animCur, false);
            anim.SetInteger(playAnim, value);
            animCur = playAnim;
        }

        public void PlayIdle()
        {
            PlayAnimation("AttackID", 0);
            anim.SetBool(animCur, false);
            animCur = "Idle";
        }

        /// <summary>
        /// 网络用播放动画
        /// </summary>
        public void NetPlayClip(string ClipName, int AttackID)
        {

            if (ClipName == "Idle")
            {
                if (AnimLock == false)
                {
                    anim.SetBool("Run", false);
                    anim.SetBool("Falling", false);
                    anim.SetBool("Defend", false);
                }
            }
            if (ClipName == "Run")
            {
                if (AnimLock == false)
                {
                    anim.SetBool("Run", true);
                }
            }

            if (ClipName == "Spear Melee Attack 02")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetTrigger("Spear Melee Attack 02");
            }
            if (ClipName == "Spin Attack")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetTrigger("Spin Attack");
            }
            if (ClipName == "Pick Up")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetTrigger("Pick Up");
            }
            if (ClipName == "Jump")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetTrigger("Jump");

            }
            if (ClipName == "Magic Shoot Attack")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetTrigger("Magic Shoot Attack");
            }
            if(ClipName=="Attack ID")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetInteger("Attack ID", AttackID);
            }
            //if (anim.GetCurrentAnimatorClipInfo(Layer)[0].clip.name != ClipName)
            //{
            //    if(ClipName=="Run" || ClipName == "Spear Melee Attack 02" || ClipName == "Spin Attack" || ClipName == "Pick Up" || ClipName == "Jump" || ClipName == "Magic Shoot Attack")
            //    {
            //        anim.SetBool(ClipName,true);
            //        //anim.SetBool("Run",true);
            //    }
            //    else
            //    {
            //        anim.SetBool(ClipName, false);
            //        //anim.SetBool("Run", false);
            //    }
            //    anim.Play(ClipName, Layer, normalizedTime);
            //}
        }

        /// <summary>
        /// 获取当前Attack ID
        /// </summary>
        /// <returns></returns>
        public int GetAttackID()
        {
            return anim.GetInteger("Attack ID");
        }
        #region 动画事件
        public void MagicShootEnd()
        {
            AnimLock = false;
        }
        public void JumpEnd()
        {
            AnimLock = false;
        }
        public void PickUpEnd()
        {
            AnimLock = false;
        }
        public void FallingEnd()
        {
            AnimLock = false;
        }
        public void SpearEnd()
        {
            AnimLock = false;
        }
        public void SpinEnd()
        {
            AnimLock = false;
        }
        public void SingleWieldAttack1End()
        {
            AnimLock = false;
        }
        public void SingleWieldAttack2End()
        {
            AnimLock = false;
        }
        public void SingleWieldAttack3End()
        {
            AnimLock = false;
        }
        public void DoubleHandsAttack1End()
        {
            AnimLock = false;
        }
        public void DoubleHandsAttack2End()
        {
            AnimLock = false;
        }
        public void DoubleHandsAttack3End()
        {
            AnimLock = false;
        }
        public void DoubleHandsAttack4End()
        {
            AnimLock = false;
        }
        public void SpinAttackStart()
        {
            AnimStartName = "Spin Attack";
        }
        public void IdleStart()
        {
            AnimStartName = "Idle";
        }
        public void MagicShootStart()
        {
            AnimStartName = "Magic Shoot Attack";
        }
        public void SpearStart()
        {
            AnimStartName = "Spear Melee Attack 02";
        }
        public void RunStart()
        {
            AnimStartName = "Run";
        }
        public void JumpStart()
        {
            AnimStartName = "Jump";
        }
        public void LeftAttackStart()
        {
            AnimStartName = "Melee Left Attack 01";
        }
        #endregion
    }

}