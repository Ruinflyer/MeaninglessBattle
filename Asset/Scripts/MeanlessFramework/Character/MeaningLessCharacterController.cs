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

        public virtual void UseGravity(float Gravity)
        {
            Vector3 moveDirection = Vector3.zero;
            if (!CC.isGrounded)
            {
                moveDirection.y -= Gravity * Time.fixedDeltaTime;
            }
            else
                moveDirection = Vector3.zero;
            CC.Move(moveDirection);
        }

        public abstract void Move(float walkSpeed);
        public abstract void Jump(float jumpSpeed);

        public virtual void EquipWeapon(int itemID, Transform LHand, Transform RHand) { }
        public virtual void EquipClothes(int itemID) { }
        public virtual void EquipHelmet(int itemID, Transform Head) { }





    }
}