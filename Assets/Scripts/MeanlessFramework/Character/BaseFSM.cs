using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meaningless
{

    public abstract class BaseFSM:MonoBehaviour
    {
        public CharacterStatus characterStatus;
        public MeaninglessCharacterController controller { get; set; }
        public AnimationManager animationManager;
        public int comboCount{ get; set; }
        public bool Attacked=false;
        public bool picked = false;
        public bool isFound = false;

        //FSM中的所有状态(多个FSMState)组成的列表
        private List<FSMState> fsmStates;

        //当前状态编号
        private FSMStateType currentStateID;
        public FSMStateType CurrentStateID { get { return currentStateID; } }

        //当前状态
        private FSMState currentState;
        public FSMState CurrentState { get { return currentState; } }

        protected virtual void Initialize()
        {
        }
        protected virtual void FSMUpdate() { }
        protected virtual void FSMFixedUpdate() { }

        public BaseFSM()
        {
            fsmStates = new List<FSMState>();
        }

        /// <summary>
        /// 向状态列表中加入一个新状态
        /// </summary>
        /// <param name="fsmState"></param>
        public void AddFSMState(FSMState fsmState)
        {
            if (fsmState == null)
                Debug.LogError("FSM ERROR: Null reference is not allowed");

            if (fsmStates.Count == 0)
            {
                fsmStates.Add(fsmState);
                currentState = fsmState;
                currentStateID = fsmState.ID;
                return;
            }
            foreach (FSMState state in fsmStates)
            {
                if (state.ID == fsmState.ID)
                {
                    Debug.LogError("FSM ERROR: Trying to add a state that was already inside the list");
                    return;
                }
            }
            fsmStates.Add(fsmState);
        }

        /// <summary>
        /// 从状态列表中删除一个状态
        /// </summary>
        /// <param name="fsmState"></param>
        public void DeleteState(FSMStateType fsmState)
        {
            foreach (FSMState state in fsmStates)
            {
                if (state.ID == fsmState)
                {
                    fsmStates.Remove(state);
                    return;
                }
            }
            Debug.LogError("FSM ERROR: The state passed was not on the list. Impossible to delete it");
        }

        /// <summary>
        /// 转换状态
        /// </summary>
        /// <param name="trans"></param>
        public void PerformTransition(FSMTransitionType trans)
        {
            FSMStateType id = currentState.GetOutputState(trans);
            currentStateID = id;

            foreach (FSMState state in fsmStates)
            {
                if (state.ID == currentStateID)
                {
                    Debug.Log(currentStateID);
                    currentState = state;
                    break;
                }
            }
        }



        public void PlayAnimation(string aniName)
        {
            animationManager.PlayAnimation(aniName);
        }

        public void PlayAnimation(string aniName,int value)
        {
            animationManager.PlayAnimation(aniName, value);
        }

        void Start()
        {
            MessageCenter.AddListener(EMessageType.FoundItem, (object obj) => { isFound = (bool)obj; });
            Initialize();
        }

        void Update()
        {
            FSMUpdate();
        }

        void FixedUpdate()
        {
            FSMFixedUpdate();
        }
    }
}