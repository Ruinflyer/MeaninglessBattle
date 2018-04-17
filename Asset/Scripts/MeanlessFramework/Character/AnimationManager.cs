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
        private string animCur;

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
    }

}