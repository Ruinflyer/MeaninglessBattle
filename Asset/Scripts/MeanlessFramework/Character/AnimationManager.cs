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
            if (anim.GetCurrentAnimatorClipInfo(Layer)[0].clip.name != ClipName)
            {
                anim.Play(ClipName, Layer, normalizedTime);
            }
        }

    }

}