using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Meaningless
{

    public abstract class FSMState
    {

        protected Dictionary<FSMTransitionType, FSMStateType> map = new Dictionary<FSMTransitionType, FSMStateType>();


        protected FSMStateType stateID;
        public FSMStateType ID { get { return stateID; } }

        

        public void AddTransition(FSMTransitionType transition, FSMStateType id)
        {
            if (map.ContainsKey(transition))
            {
                Debug.LogWarning("ERROR: transition is already inside the map");
                return;
            }
            map.Add(transition, id);
            Debug.Log("Added : " + transition + " with ID : " + id);
        }


        public void DeleteTransition(FSMTransitionType trans)
        {
            if (map.ContainsKey(trans))
            {
                map.Remove(trans);
                return;
            }
            Debug.LogError("ERROR: Transition passed was not on this State's List");
        }

        public FSMStateType GetOutputState(FSMTransitionType trans)
        {
            return map[trans];
        }


        public abstract void Reason(BaseFSM FSM);


        public abstract void Act(BaseFSM FSM);
    }
}