using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meaningless
{

    public abstract class MeaninglessCharacterController : MonoBehaviour
    {

        public CharacterController CC;
        public float Gravity=9.8f;
        public int CurrentSelected=1;
        public float deBuffTime = 0;
        
        public List<NetworkPlayer> List_CanAttack = new List<NetworkPlayer>();


        protected bool deBuffFlag;

        public List<Buff> buffList = new List<Buff>();

        public Dictionary<string, NetworkPlayer> ScenePlayers = new Dictionary<string, NetworkPlayer>();


        //测试用身体坐标
        public Transform LHand;
        public Transform RHand;
        public Transform Head;
        public Transform Wings;

        public enum Body
        {
            LHand,
            RHand,
            Head
        }

        protected virtual void Initialize() { }
        protected virtual void CCUpdate() { }
        protected virtual void CCFixedUpdate() { }

        void Start()
        {
            this.CC = this.GetComponent<CharacterController>();
            ScenePlayers = GameObject.Find("NetworkPlayerManager").GetComponent<NetworkPlayerManager>().ScenePlayers;
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

        public void OpenBag()
        {
            if(Input.GetButtonDown("Bag"))
            {
                UIManager.Instance.ShowUI(UIid.BagUI);

                CameraBase.Instance.isFollowing = false;

            }
        }
        public void Esc()
        {
            if(Input.GetButtonDown("Esc"))
            {
                UIManager.Instance.ShowUI(UIid.EscapeUI);
                CameraBase.Instance.isFollowing = false;
            }
        }

        public abstract void Move(float walkSpeed, float jumpSpeed);
        public abstract void FallingCtrl(float Speed);
        public abstract void Jump(float jumpSpeed);
        public abstract bool CheckCanAttack(GameObject center, GameObject enemy, float distance, float angle);
        public abstract void GetDeBuffInTime(BuffType debuff, float time,CharacterStatus status);

        public virtual void ChangeWeapon(int currentSelected) { }
        public virtual void FindTranform(Body type ) { }
        public virtual SingleItemInfo GetCurSelectedWeaponInfo() { return null; }



    }
}