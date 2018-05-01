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
        public void NetPlayClip(int Layer, string ClipName, float normalizedTime = 0f)
        {

            if (ClipName == "Idle")
            {
                if (AnimLock == false)
                {
                    anim.SetBool("Run", false);
                    anim.SetBool("Spear Melee Attack 02", false);
                    anim.SetBool("Spin Attack", false);
                    anim.SetBool("Pick Up", false);
                    anim.SetBool("Jump", false);
                    anim.SetBool("Magic Shoot Attack", false);
                }
            }
            if (ClipName == "Run")
            {
                if (AnimLock == false)
                {
                    anim.SetBool("Run", true);
                    anim.SetBool("Spear Melee Attack 02", false);
                    anim.SetBool("Spin Attack", false);
                    anim.SetBool("Pick Up", false);
                    anim.SetBool("Jump", false);
                    anim.SetBool("Magic Shoot Attack", false);
                }
            }

            if (ClipName == "Spear Melee Attack 02")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetBool("Spear Melee Attack 02", true);
                anim.SetBool("Spin Attack", false);
                anim.SetBool("Pick Up", false);
                anim.SetBool("Jump", false);
                anim.SetBool("Magic Shoot Attack", false);
            }
            if (ClipName == "Spin Attack")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetBool("Spear Melee Attack 02", false);
                anim.SetBool("Spin Attack", true);
                anim.SetBool("Pick Up", false);
                anim.SetBool("Jump", false);
                anim.SetBool("Magic Shoot Attack", false);
            }
            if (ClipName == "Pick Up")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetBool("Spear Melee Attack 02", false);
                anim.SetBool("Spin Attack", false);
                anim.SetBool("Pick Up", true);
                anim.SetBool("Jump", false);
                anim.SetBool("Magic Shoot Attack", false);
            }
            if (ClipName == "Jump")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetBool("Spear Melee Attack 02", false);
                anim.SetBool("Spin Attack", false);
                anim.SetBool("Pick Up", false);
                anim.SetBool("Jump", true);
                anim.SetBool("Magic Shoot Attack", false);
            }
            if (ClipName == "Magic Shoot Attack")
            {
                AnimLock = true;
                anim.SetBool("Run", false);
                anim.SetBool("Spear Melee Attack 02", false);
                anim.SetBool("Spin Attack", false);
                anim.SetBool("Pick Up", false);
                anim.SetBool("Jump", false);
                anim.SetBool("Magic Shoot Attack", true);
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
        #endregion
    }

}