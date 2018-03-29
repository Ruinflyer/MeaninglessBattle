using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meaningless
{

    public abstract class MeaninglessCharacterController : MonoBehaviour
    {

        public CharacterController CC;
        public float Gravity=9.8f;

        protected virtual void Initialize() { }
        protected virtual void CCUpdate() { }
        protected virtual void CCFixedUpdate() { }

        void Start()
        {
            this.CC = this.GetComponent<CharacterController>();
            Initialize();
        }


        void Update()
        {
            CCUpdate();
        }

        void FixedUpdate()
        {
           CCFixedUpdate();
        }

        public abstract void Move(float walkSpeed);
        public abstract void Attack();
        public abstract void UseGravity(float Gravity);
        public abstract void Jump(float jumpSpeed);

    }
}